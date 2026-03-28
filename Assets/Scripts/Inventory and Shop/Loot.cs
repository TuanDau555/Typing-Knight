using UnityEngine;

public class Loot : MonoBehaviour
{
    #region Paremeters
    
    [Header("Item Data")]
    [SerializeField] private ItemSO itemSO;

    // Add Item animater here if it has 
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int quantity;

    #endregion

    #region Execute

    private void OnValidate()
    {
        if(itemSO == null)
        {
            Debug.LogError("Missing itemSO Reference");
            return;
        }

        spriteRenderer.sprite = itemSO.itemIcon;
        this.name = itemSO.name;
    }

    private void OnMouseEnter()
    {
        if(InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager missing");
            return;
        }

        InventoryManager.Instance.AddItem(itemSO, quantity);
        Destroy(gameObject);
    }

    #endregion
}