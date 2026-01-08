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
    private void Initialize() //실행되고 상태 초기화
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
    public void RecordTraitPurchase(int stageIndex) //스테이지 별 구매횟수 기록
    {
        if (stages == null || purchaseCounts == null) return;

        if (stageIndex < 0 || stageIndex >= purchaseCounts.Length) return;

        
        purchaseCounts[stageIndex]++;

        
        if (stageIndex != currentStageIndex) return;

        
        int need = stages[currentStageIndex].purchasesToUnlockNextStage;
        if (need <= 0) need = 1; 

        if (purchaseCounts[currentStageIndex] >= need) // 다음 스테이지 개방 조건 
        {
            UnlockNextStage();
        }
    }
    public void UnlockNextStage() // 다음 스테이지 개방 
    {
        if (isNextStageUnlocked) return;

        isNextStageUnlocked = true;
    }

    public bool CanAdvance() // 스테이지 개방 조건  
    {
        if (stages == null) return false;
        if (currentStageIndex + 1 >= stages.Length) return false;

        if (!isNextStageUnlocked) return false;

        return true;
    }

    public void AdvanceStage() // 다음 단계
    {
        if (stages == null) return;
        int nextIndex = currentStageIndex + 1;
        if (nextIndex < 0 || nextIndex >= stages.Length) return;

        currentStageIndex = nextIndex;

        isNextStageUnlocked = false;
    }
}


