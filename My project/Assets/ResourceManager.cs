using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    // ===== Inspector (State) =====
    [Header("State (Debug/Inspect)")]
    [SerializeField] private int adaptation = 0;
    [SerializeField] private int clickPower = 1;

    
    public int Adaptation => adaptation;
    public int ClickPower => clickPower;

    
    public void AddFromClick()
    {
        
        if (clickPower <= 0) return;

        adaptation += clickPower;

        
    }

    
    public bool TrySpend(int amount)
    {
        if (amount <= 0) return true;          
        if (adaptation < amount) return false; 

        adaptation -= amount;

        
        if (adaptation < 0) adaptation = 0;

        
        return true;
    }

    
    public void IncreaseClickPower(int delta)
    {
        
        clickPower += delta;
        if (clickPower < 1) clickPower = 1;

        
    }
}