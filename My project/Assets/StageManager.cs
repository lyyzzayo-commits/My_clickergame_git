using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private StageData[] stages;

    [Header("State")]
    [SerializeField] private int currentStageIndex = 0;
    [SerializeField] private bool isNextStageUnlocked = false;

    private int[] purchaseCounts;

    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        if (stages == null || stages.Length == 0)
        {
            purchaseCounts = new int[0];
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

        // 3) 구매 횟수 기록
        purchaseCounts[stageIndex]++;

        // 4) (핵심) "현재 단계 구매"일 때만 언락 조건 검사
        if (stageIndex != currentStageIndex) return;

        // 5) 현재 단계의 언락 조건 도달 시 다음 단계 언락
        int need = stages[currentStageIndex].purchasesToUnlockNextStage;
        if (need <= 0) need = 1; // 방어: 0/음수면 1로 취급(정책)

        if (purchaseCounts[currentStageIndex] >= need)
        {
            UnlockNextStage();
        }
    }
    public void UnlockNextStage()
    {
        if (isNextStageUnlocked) return;

        isNextStageUnlocked = true;
    }

    public bool CanAdvance()
    {
        if (stages == null) return false;
        if (currentStageIndex + 1 >= stages.Length) return false;

        if (!isNextStageUnlocked) return false;

        return true;
    }

    public void AdvanceStage()
    {
        if (stages == null) return;
        int nextIndex = currentStageIndex + 1;
        if (nextIndex < 0 || nextIndex >= stages.Length) return;

        currentStageIndex = nextIndex;

        isNextStageUnlocked = false;
    }
}


