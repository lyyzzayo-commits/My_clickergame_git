using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Trait List")]
    [SerializeField] private Transform traitListRoot;       // TraitList(VerticalLayoutGroup 붙은 곳)
    
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private PurchaseManager purchase;

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

        // ResourceManager�� �̺�Ʈ�� �ִٸ� ���⿡ ����
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

    // 1) ����/�ε� �� 1ȸ ��ü ����
    public void RefreshAll()
    {
        RefreshAdaptationText();
        RefreshStagePanel();
        RefreshUpgradePanel();
        RefreshStageIcons();
    }
    

    // 2) StageManager.OnStageChanged���� ȣ���
    private void HandleStageChanged(StageData newStage)
    {
        RefreshStagePanel();     // �̸�/����/�̹���
        RefreshUpgradePanel();   // ���� �������� ���/��ư ����
        RefreshStageIcons();     // ������ ��
    }

    // 3) Adaptation ���� �� ȣ��ǵ���(�̺�Ʈ�� ������ BuyButton Ŭ�� �� ���� ȣ���ص� ��)
    public void HandleAdaptationChanged(int newValue)
    {
        RefreshAdaptationText(newValue);
        RefreshUpgradePanel(); // ���� �ٲ�� ���� ���� ���ε� �ٲ�
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
        if (resourceManager == null || stageManager == null || purchase == null) return;

        int cost = purchase.CurrentCost;
        int clickPowerGain = purchase.CurrentClickPowerGain;

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
    private void BuildTraitButtons()
{
    int count = stageManager.StageCount;

    for (int i = 0; i < count; i++)
    {
        var data = stageManager.GetStage(i);
        if (data == null) continue;

        var btn = Instantiate(traitButtonPrefab, traitListRoot);
        btn.Bind(i, data, this, stageManager, resourceManager);
        spawned.Add(btn);
    }
}

}
