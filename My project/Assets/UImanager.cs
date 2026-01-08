using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private BuyButton buyButton; 


    // UI 참조
    [SerializeField] private TMP_Text adaptationText;
    [SerializeField] private TMP_Text stageNameText;
    [SerializeField] private TMP_Text stageDescText;
    [SerializeField] private Image stageImage;

    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private TMP_Text clickPowerText;
    [SerializeField] private Button upgradeButton;

    [SerializeField] private StageManager stageManager;
    [SerializeField] private Image[] stageIcons;

    [SerializeField] private Color lockedColor = Color.black;
    [SerializeField] private Color unlockedColor = Color.white;


    //string stageName = stageManager.
    //string currentStageDesc = stageManager.
    //int currentUpgradeCost = stageManager.

    private void Start()
    {
        RefreshStageIcons();
    }

    

    public void RefreshAll() // 현재 게임 상태 전체를 한 번에 화면에 반영
    {
        int adaptation = resourceManager.Adaptation;
        adaptationText.text = adaptation.ToString();

        
       //if (stageNameText != null)
            //stageNameText.text = stageName;

        
        //if (stageDescText != null)
            //stageDescText.text = currentStageDesc;

        //이건 이미지 형태로 변수를 생성해야하는데 어떻게 하는지 모르겠음
        //if (stageImage != null)
            //stageImage.sprite = currentStageSprite;

        
        //if (upgradeCostText != null)
            //upgradeCostText.text = currentUpgradeCost.ToString();

        //if (clickPowerText != null)
            //clickPowerText.text = resourceManager.ClickPower.ToString();

        //bool affordable = adaptation >= currentUpgradeCost;
        //upgradeButton.interactable = affordable;
    }

    public void SetAdaptation(int value) // 외부에서 전달 받은 Adaptation 값으로 UI 갱신
    {
        if (adaptationText != null)
            adaptationText.text = value.ToString();
        if (upgradeButton != null)
        {
            //bool affordable = value >= currentUpgradeCost;
            //upgradeButton.interactable = affordable;
        }
    }
    public void RefreshAdaptation() //Adaptation 값이 바뀌었을 때 UI만 부분 갱신
    {
        int adaptation = resourceManager.Adaptation;

        if (adaptationText != null)
            adaptationText.text = adaptation.ToString() ;
        if (upgradeButton != null)
        {
            //bool affordable = adaptation >= currentUpgradeCost;
            //upgradeButton.interactable = affordable;
        }
    }

    public void SetStage(string name,string desc,Sprite sprite) // Stage가 바뀌었을 때 Stage UI만 교체
    {
        //if (stageNameText != null)
            //stageNameText.text = stageName;

        //if (stageDescText != null)
            //stageDescText.text = currentStageDesc;

        //if (stageImage != null)
            //stageImage.sprite = stageSprite;
    }

    public void SetUpgrade(int cost, int clickPower, bool affordable, bool canAdvance) // 업그레이드 정보(비용/효과)를 UI에 반영
    {
        //currentUpgradeCost = cost;

        if (upgradeCostText != null)
            upgradeCostText.text = cost.ToString();
        
        if (clickPowerText != null)
            clickPowerText.text = clickPower.ToString();

        if (upgradeButton != null)
            upgradeButton.interactable = affordable && canAdvance;
    }

    private void SetUpgradeAffordable(bool affordable) // 업그레이드 버튼의 구매 가능/불가 상태만 제어
    {
        if (upgradeButton != null)

            upgradeButton.interactable = affordable;
    }

    public void RefreshStageIcons()
    {
        if (stageManager == null) return;

        for (int i = 0; i < stageIcons.Length; i++)
        {
            var img = stageIcons[i];
            if (img == null) continue;

            StageData data = stageManager.GetStage(i);
            if (data == null)
            {
                img.enabled = false;
                continue;
            }

            img.enabled = true;
            img.sprite = data.stageSprite; // StageData의 스프라이트 필드명 맞춰서
            bool unlocked = stageManager.IsStageUnlocked(i);

            img.color = unlocked ? unlockedColor : lockedColor;
        }
    }

    private void OnEnable()
    
    {
        if (stageManager != null)
            stageManager.OnStageVisualDirty += RefreshStageIcons;
    }

    private void OnDisable()
    {
        if (stageManager != null)
            stageManager.OnStageVisualDirty -= RefreshStageIcons;
    }


}
