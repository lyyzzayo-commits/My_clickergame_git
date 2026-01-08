using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;

    private void Awake()
    {
        if (GetComponent<Collider>() == null && GetComponent<Collider2D>() == null)
        {
            Debug.LogWarning($"{name} has ClickHandler but no Collider.");
        }
    }
    
    public void OnClick_character()
    {
        resourceManager.AddFromClick();
        
    }
}
