using System;
using UnityEngine;

/// <summary>
/// PurchaseManager
/// - "어느 단계든 반복 구매"를 처리한다.
/// - 결제/강화는 ResourceManager에게 위임한다.
/// - 구매 횟수 저장/언락 판단은 StageManager에게 위임한다.
/// - (추가 규칙) "다음 스테이지( current+1 )를 처음 구매"하면 구매 성공 후 StageManager.AdvanceStage()로 진입한다.
/// - UI/외형 변경/사운드는 하지 않는다(필요하면 이벤트로만 알린다).
/// </summary>
[DisallowMultipleComponent]
public sealed class PurchaseManager : MonoBehaviour
{
    [Header("Dependencies (Required)")]
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private StageManager stageManager;

    public event Action<int> OnPurchaseSucceeded;
    public event Action<int> OnPurchaseFailed;

    public int CurrentCost
    {
        get
        {
            if (stageManager == null) return 0;
            int nextIndex = stageManager.CurrentStageIndex + 1;
            if (!IsValidStageIndex(nextIndex)) return 0;

            StageData data = stageManager.GetStage(nextIndex);
            return data != null ? Mathf.Max(0, data.traitCost) : 0;
        }
    }

    public int CurrentClickPowerGain
    {
        get
        {
            if (stageManager == null) return 0;
            int nextIndex = stageManager.CurrentStageIndex + 1;
            if (!IsValidStageIndex(nextIndex)) return 0;

            StageData data = stageManager.GetStage(nextIndex);
            return data != null ? data.traitClickGain : 0;
        }
    }
    private void Awake()
    {
        if (resourceManager == null) resourceManager = FindFirstObjectByType<ResourceManager>();
        if (stageManager == null) stageManager = FindFirstObjectByType<StageManager>();
    }

    /// <summary>
    /// UI 버튼(PurchaseRequestor)에서 호출.
    /// </summary>
    public void RequestPurchase(int stageIndexToBuy)
    {
        if (!ValidateRefs()) { OnPurchaseFailed?.Invoke(stageIndexToBuy); return; }

        // 0) index 검증
        if (!IsValidStageIndex(stageIndexToBuy))
        {
            Debug.LogError($"[PurchaseManager] Invalid stageIndexToBuy={stageIndexToBuy}");
            OnPurchaseFailed?.Invoke(stageIndexToBuy);
            return;
        }

        // 1) 잠김이면 구매 불가 (현재 이하 = 항상 가능, 다음 1단계 = 언락일 때만 가능)
        if (!stageManager.IsStageUnlocked(stageIndexToBuy))
        {
            OnPurchaseFailed?.Invoke(stageIndexToBuy);
            return;
        }

        // 2) "다음 스테이지 첫 구매면 구매 성공 후 어드밴스" 여부를 구매 전에 계산
        int current = stageManager.CurrentStageIndex;
        bool isBuyingNextStage = (stageIndexToBuy == current + 1);
        bool isFirstPurchaseOfThatStage = (stageManager.GetPurchaseCount(stageIndexToBuy) == 0);
        bool shouldAdvanceAfterPurchase = isBuyingNextStage && isFirstPurchaseOfThatStage;

        // 3) 구매 데이터 조회
        StageData data = stageManager.GetStage(stageIndexToBuy);
        if (data == null)
        {
            Debug.LogError($"[PurchaseManager] StageData is null at index={stageIndexToBuy}");
            OnPurchaseFailed?.Invoke(stageIndexToBuy);
            return;
        }

        int cost = Mathf.Max(0, data.traitCost);
        int gain = data.traitClickGain;

        // 4) 결제 시도
        if (!resourceManager.TrySpend(cost))
        {
            OnPurchaseFailed?.Invoke(stageIndexToBuy);
            return;
        }

        // 5) 구매 성공 처리
        resourceManager.IncreaseClickPower(gain);
        stageManager.RecordTraitPurchase(stageIndexToBuy); // 여기서 "현재 스테이지 구매 누적"이면 언락까지 처리됨

        // 6) (추가 규칙) 다음 스테이지를 "첫 구매"했다면 스테이지 진입
        if (shouldAdvanceAfterPurchase)
            stageManager.AdvanceStage();

        OnPurchaseSucceeded?.Invoke(stageIndexToBuy);
    }

    private bool ValidateRefs()
    {
        if (resourceManager == null)
        {
            Debug.LogError("[PurchaseManager] ResourceManager reference missing.");
            return false;
        }
        if (stageManager == null)
        {
            Debug.LogError("[PurchaseManager] StageManager reference missing.");
            return false;
        }
        return true;
    }

    private bool IsValidStageIndex(int index)
    {
        return index >= 0 && index < stageManager.StageCount;
    }
}
