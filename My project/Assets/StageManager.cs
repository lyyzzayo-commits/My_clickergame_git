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
    private void Initialize() //½ÇÇàµÇ°í »óÅÂ ÃÊ±âÈ­
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
    public void RecordTraitPurchase(int stageIndex) //½ºÅ×ÀÌÁö º° ±¸¸ÅÈ½¼ö ±â·Ï
    {
        if (stages == null || purchaseCounts == null) return;

        if (stageIndex < 0 || stageIndex >= purchaseCounts.Length) return;

        
        purchaseCounts[stageIndex]++;

        
        if (stageIndex != currentStageIndex) return;

        
        int need = stages[currentStageIndex].purchasesToUnlockNextStage;
        if (need <= 0) need = 1; 

        if (purchaseCounts[currentStageIndex] >= need) // ´ÙÀ½ ½ºÅ×ÀÌÁö °³¹æ Á¶°Ç 
        {
            UnlockNextStage();
        }
    }
    public void UnlockNextStage() // ´ÙÀ½ ½ºÅ×ÀÌÁö °³¹æ 
    {
        if (isNextStageUnlocked) return;

        isNextStageUnlocked = true;
        OnStageVisualDirty?.Invoke();

    }

    public bool CanAdvance() // ½ºÅ×ÀÌÁö °³¹æ Á¶°Ç  
    {
        if (stages == null) return false;
        if (currentStageIndex + 1 >= stages.Length) return false;

        if (!isNextStageUnlocked) return false;

        return true;
    }

    public void AdvanceStage() // ´ÙÀ½ ´Ü°è
    {
        if (stages == null) return;
        int nextIndex = currentStageIndex + 1;
        if (nextIndex < 0 || nextIndex >= stages.Length) return;

        currentStageIndex = nextIndex;

        isNextStageUnlocked = false;
        OnStageVisualDirty?.Invoke();

    }

    //UIê°€ ì½ì„ ìˆ˜ ìˆë„ë¡
    
    public event System.Action OnStageVisualDirty;
    public int StageCount => stages != null ? stages.Length : 0;
    public int CurrentStageIndex => currentStageIndex;

    public StageData GetStage(int index)
    {
        if (stages == null) return null;
        if (index < 0 || index >= stages.Length) return null;
        return stages[index];
    }

    // ì ê¹€/ì–¸ë½ íŒì •: ì§€ê¸ˆ êµ¬ì¡°(ë‹¤ìŒ 1ë‹¨ê³„ë§Œ ì–¸ë½ ê°€ëŠ¥)ì— ë§ì¶˜ ë²„ì „
    public bool IsStageUnlocked(int index)
    {
        if (stages == null) return false;
        if (index < 0 || index >= stages.Length) return false;

        if (index <= currentStageIndex) return true;
        if (index == currentStageIndex + 1) return isNextStageUnlocked;
        return false;
    }
}


