using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private StageData stageData;

    // UI 참조
    [SerializeField] private TMP_Text adaptationText;
    [SerializeField] private TMP_Text stageNameText;
    [SerializeField] private TMP_Text stageDescText;
    [SerializeField] private Image stageImage;

    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private TMP_Text clickPowerText;
    [SerializeField] private Button upgradeButton;

    string stageName = stageData.
    string currentStageDesc = stageData.
    int currentUpgradeCost = stageData.


    public void RefreshAll()
    {
        int adaptation = resourceManager.Adaptation;
        adaptationText.text = adaptation.ToString();

        
        if (stageNameText != null)
            stageNameText.text = stageName;

        
        if (stageDescText != null)
            stageDescText.text = currentStageDesc;

        //이건 이미지 형태로 변수를 생성해야하는데 어떻게 하는지 모르겠음
        if (stageImage != null)
            stageImage.sprite = currentStageSprite;

        
        if (upgradeCostText != null)
            upgradeCostText.text = currentUpgradeCost.ToString();

        if (clickPowerText != null)
            clickPowerText.text = resourceManager.ClickPower.ToString();

        bool affordable = adaptation >= currentUpgradeCost;
        upgradeButton.interactable = affordable;
    }

    public void SetAdaptation(int value)
    {
        if (adaptationText != null)
            adaptationText.text = value.ToString();
        if (upgradeButton != null)
        {
            bool affordable = value >= currentUpgradeCost;
            upgradeButton.interactable = affordable;
        }
    }
    public void RefreshAdaptation()
    {
        int adaptation = resourceManager.Adaptation;

        if (adaptationText != null)
            adaptationText.text = adaptation.ToString() ;
        if (upgradeButton != null)
        {
            bool affordable = adaptation >= currentUpgradeCost;
            upgradeButton.interactable = affordable;
        }
    }

    public void SetStage(string name,string desc,Sprite sprite)
    {
        if (stageNameText != null)
            stageNameText.text = stageName;

        if (stageDescText != null)
            stageDescText.text = currentStageDesc;

        if (stageImage != null)
            stageImage.sprite = stageSprite;
    }

    private void SetUpgrade(int cost, int clickPower)
    {
        currentUpgradeCost = cost;

        if (upgradeCostText != null)
            upgradeCostText.text = cost.ToString();
        
        if (clickPowerText != null)
            clickPowerText.text = clickPower.ToString();

        if (upgradeButton != null)
        {
            int adaptation = resourceManager.Adaptation;
            bool affordable = adaptation >= cost;
            upgradeButton.interactable = affordable;
        }
    }

    private void SetUpgradeAffordable(bool affordable)
    {
        if (upgradeButton == null)

            upgradeButton.interactable = affordable;
    }
}
