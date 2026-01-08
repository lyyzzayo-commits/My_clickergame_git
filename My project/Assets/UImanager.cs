using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Trait List")]
    [SerializeField] private Transform traitListRoot;       // TraitList(VerticalLayoutGroup 붙은 곳)

    [Header("Auto Spawn")]
    [SerializeField] private GameObject traitButtonPrefab;
    [SerializeField] private int spawnCount = 40;
    private readonly List<GameObject> spawned = new();
    private bool spawnedOnce;
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

    [Header("Upgrade Button Visual")]
    [SerializeField] private Image upgradeButtonImage;

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
        if (resourceManager != null)
            resourceManager.OnAdaptationChanged += HandleAdaptationChanged;

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

        if (resourceManager != null)
            resourceManager.OnAdaptationChanged -= HandleAdaptationChanged;

        // resourceManager.OnAdaptationChanged -= HandleAdaptationChanged;
    }

    private void Start()
    {
        SpawnTraitButtonsOnce();
        RefreshAll();
    }

    private void SpawnTraitButtonsOnce()
    {
        if (spawnedOnce) return;
        if (traitListRoot == null)
        {
            Debug.LogError("[UIManager] traitListRoot(Content) 미연결");
            return;
        }
        if (traitButtonPrefab == null)
        {
            Debug.LogError("[UIManager] traitButtonPrefab 미연결");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject go = Instantiate(traitButtonPrefab, traitListRoot);
            spawned.Add(go);

            // PurchaseRequestor stageIndex 자동 세팅 (0~39)
            var req = go.GetComponent<PurchaseRequestor>();
            if (req != null)
            {
                req.SetStageIndex(i);
            }
            else
            {
                Debug.LogWarning($"[UIManager] PurchaseRequestor가 프리팹에 없음: {traitButtonPrefab.name}");
            }
        }

        // 레이아웃 즉시 갱신(세로정렬/스크롤 반영)
        LayoutRebuilder.ForceRebuildLayoutImmediate(traitListRoot as RectTransform);

        spawnedOnce = true;
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

        var stage = stageManager.CurrentStage;
        if (stage != null && upgradeButtonImage != null)
        {
            upgradeButtonImage.sprite = stage.stageSprite;
        }
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
