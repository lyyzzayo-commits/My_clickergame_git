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

    //string stageName = stageManager.
    //string currentStageDesc = stageManager.
    //int currentUpgradeCost = stageManager.
    

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

    private void SetUpgrade(int cost, int clickPower) // 업그레이드 정보(비용/효과)를 UI에 반영
    {
        //currentUpgradeCost = cost;

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

    private void SetUpgradeAffordable(bool affordable) // 업그레이드 버튼의 구매 가능/불가 상태만 제어
    {
        if (upgradeButton == null)

            upgradeButton.interactable = affordable;
    }
}
