using UnityEngine;

[DisallowMultipleComponent]
public sealed class PurchaseRequestor : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("이 버튼이 구매 요청하는 단계 인덱스 (예: 0=1단계, 1=2단계...)")]
    [SerializeField] private int stageIndex = 0;

    [Tooltip("구매를 실제 처리하는 매니저. 비워두면 런타임에 자동 탐색(FindFirstObjectByType)")]
    [SerializeField] private PurchaseManager purchaseManager;

    private void Awake()
    {
        // 인스펙터에 안 넣었으면 자동으로 찾는다(씬에 1개 있다고 가정)
        if (purchaseManager == null)
            purchaseManager = FindFirstObjectByType<PurchaseManager>();
    }

    /// <summary>
    /// UI Button.onClick에서 호출할 함수
    /// </summary>
    public void RequestPurchase()
    {
        if (purchaseManager == null)
        {
            Debug.LogError($"[PurchaseRequestor] PurchaseManager가 연결되지 않았습니다. ({name})");
            return;
        }

        if (stageIndex < 0)
        {
            Debug.LogError($"[PurchaseRequestor] stageIndex가 음수입니다. ({name})");
            return;
        }

        // PurchaseManager에 "이 단계 구매 요청" 전달
        purchaseManager.RequestPurchase(stageIndex);
    }

#if UNITY_EDITOR
    // 인스펙터에서 값 바꿀 때 자동 보정/검증
    private void OnValidate()
    {
        if (stageIndex < 0) stageIndex = 0;
    }
#endif
}