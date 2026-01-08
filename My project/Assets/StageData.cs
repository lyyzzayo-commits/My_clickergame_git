using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage Data", fileName = "StageData_")]
public class StageData : ScriptableObject
{
    [Header("Display")]
    public string stageName;
    [TextArea(2, 4)]
    public string description;
    public Sprite stageSprite;

    [Header("Trait")]
    public int traitCost;
    public int traitClickGain;

    [Header("Unlock")]
    public int purchasesToUnlockNextStage;

}