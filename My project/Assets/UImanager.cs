using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private BuyButton buyButton;

    [Header("HUD")]
    [SerializeField] private TMP_Text adaptationText;

    [Header("Stage Panel")]
    [SerializeField] private TMP_Text stageNameText;
    [SerializeField] private TMP_Text stageDescText;
    [SerializeField] private Image stageImage;

    [Header("Upgrade Panel")]
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private TMP_Text clickPowerText;
    [SerializeField] private Button upgradeButton;

    [Header("Stage Icons")]
    [SerializeField] private Image[] stageIcons;
    [SerializeField] private Color lockedColor = Color.black;
    [SerializeField] private Color unlockedColor = Color.white;

    private void OnEnable()
    {
        if (stageManager != null)
        {
            stageManager.OnStageChanged += HandleStageChanged;
            stageManager.OnStageVisualDirty += RefreshStageIcons;
        }

        // ResourceManager에 이벤트가 있다면 여기에 구독
        // resourceManager.OnAdaptationChanged += HandleAdaptationChanged;
    }

    private void OnDisable()
    {
        if (stageManager != null)
        {
            stageManager.OnStageChanged -= HandleStageChanged;
            stageManager.OnStageVisualDirty -= RefreshStageIcons;
        }

        // resourceManager.OnAdaptationChanged -= HandleAdaptationChanged;
    }

    private void Start()
    {
        RefreshAll();
    }

    // 1) 시작/로드 시 1회 전체 갱신
    public void RefreshAll()
    {
        RefreshAdaptationText();
        RefreshStagePanel();
        RefreshUpgradePanel();
        RefreshStageIcons();
    }

    // 2) StageManager.OnStageChanged에서 호출됨
    private void HandleStageChanged(StageData newStage)
    {
        RefreshStagePanel();     // 이름/설명/이미지
        RefreshUpgradePanel();   // 다음 스테이지 비용/버튼 상태
        RefreshStageIcons();     // 아이콘 색
    }

    // 3) Adaptation 변경 시 호출되도록(이벤트가 없으면 BuyButton 클릭 후 수동 호출해도 됨)
    public void HandleAdaptationChanged(int newValue)
    {
        RefreshAdaptationText(newValue);
        RefreshUpgradePanel(); // 돈이 바뀌면 구매 가능 여부도 바뀜
    }

    private void RefreshAdaptationText()
    {
        if (resourceManager == null) return;
        RefreshAdaptationText(resourceManager.Adaptation);
    }

    private void RefreshAdaptationText(int value)
    {
        if (adaptationText != null) adaptationText.text = value.ToString();
    }

    private void RefreshStagePanel()
    {
        if (stageManager == null) return;
        var stage = stageManager.CurrentStage;
        if (stage == null) return;

        if (stageNameText != null) stageNameText.text = stage.stageName;
        if (stageDescText != null) stageDescText.text = stage.description;
        if (stageImage != null) stageImage.sprite = stage.stageSprite;
    }

    private void RefreshUpgradePanel()
    {
        if (resourceManager == null || stageManager == null || buyButton == null) return;

        int cost = buyButton.CurrentCost;
        int clickPowerGain = buyButton.CurrentClickPowerGain;

        bool canAdvance = stageManager.CanAdvance();
        bool affordable = resourceManager.Adaptation >= cost;

        if (upgradeCostText != null) upgradeCostText.text = cost.ToString();
        if (clickPowerText != null) clickPowerText.text = clickPowerGain.ToString();
        if (upgradeButton != null) upgradeButton.interactable = affordable && canAdvance;
    }

    public void RefreshStageIcons()
    {
        if (stageManager == null || stageIcons == null) return;

        for (int i = 0; i < stageIcons.Length; i++)
        {
            var img = stageIcons[i];
            if (img == null) continue;

            var data = stageManager.GetStage(i);
            if (data == null)
            {
                img.enabled = false;
                continue;
            }

            img.enabled = true;
            img.sprite = data.stageSprite;

            bool unlocked = stageManager.IsStageUnlocked(i);
            img.color = unlocked ? unlockedColor : lockedColor;
        }
    }
}
