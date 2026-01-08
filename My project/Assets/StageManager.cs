using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private StageData[] stages;

    [Header("State")]
    [SerializeField] private int currentStageIndex = 0;
    [SerializeField] private bool isNextStageUnlocked = false;

    [SerializeField] private int[] purchaseCounts;

    public StageData CurrentStage
    {
        get
        {
            if (stages == null || stages.Length == 0) return null;
            if (currentStageIndex < 0 || currentStageIndex >= stages.Length) return null;
            return stages[currentStageIndex];
        }
    }

    // 의미 분리된 이벤트
    public event Action OnNextStageUnlocked;         // 언락됨
    public event Action<StageData> OnStageChanged;   // 스테이지 바뀜
    public event Action OnStageVisualDirty;          // UI가 전체 다시 그려야 함(선택)

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (stages == null || stages.Length == 0)
        {
            purchaseCounts = Array.Empty<int>();
            currentStageIndex = 0;
            isNextStageUnlocked = false;
            return;
        }

        purchaseCounts = new int[stages.Length];

        if (currentStageIndex < 0 || currentStageIndex >= stages.Length)
            currentStageIndex = 0;

        isNextStageUnlocked = false;
    }

    public void RecordTraitPurchase(int stageIndex)
    {
        if (stages == null || purchaseCounts == null) return;
        if (stageIndex < 0 || stageIndex >= purchaseCounts.Length) return;

        purchaseCounts[stageIndex]++;

        if (stageIndex != currentStageIndex) return;

        int need = stages[currentStageIndex].purchasesToUnlockNextStage;
        if (need <= 0) need = 1;

        if (!isNextStageUnlocked && purchaseCounts[currentStageIndex] >= need)
        {
            UnlockNextStage();
        }
    }

    public void UnlockNextStage()
    {
        if (isNextStageUnlocked) return;

        isNextStageUnlocked = true;
        OnNextStageUnlocked?.Invoke();
        OnStageVisualDirty?.Invoke();
    }

    public bool CanAdvance()
    {
        if (stages == null) return false;
        if (currentStageIndex + 1 >= stages.Length) return false;
        return isNextStageUnlocked;
    }

    public void AdvanceStage()
    {
        if (!CanAdvance()) return;

        currentStageIndex++;
        isNextStageUnlocked = false;

        OnStageChanged?.Invoke(CurrentStage);
        OnStageVisualDirty?.Invoke();
    }

    public int StageCount => stages != null ? stages.Length : 0;
    public int CurrentStageIndex => currentStageIndex;
    public int GetPurchaseCount(int index)
    {
        if (purchaseCounts == null) return 0;
        if (index < 0 || index >= purchaseCounts.Length) return 0;
        return purchaseCounts[index];
    }

    public StageData GetStage(int index)
    {
        if (stages == null) return null;
        if (index < 0 || index >= stages.Length) return null;
        return stages[index];
    }

    public bool IsStageUnlocked(int index)
    {
        if (stages == null) return false;
        if (index < 0 || index >= stages.Length) return false;

        if (index <= currentStageIndex) return true;
        if (index == currentStageIndex + 1) return isNextStageUnlocked;
        return false;
    }
}
