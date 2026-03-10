using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script need to attach to single slot of the inventory
/// </summary>
public class InventorySlotUI : MonoBehaviour
{
    #region Parameters
    
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI quantityText;

    #endregion

    #region Set Item

    public void SetEmpty()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        quantityText.text = "";
    }

    public void SetItem(ItemStack stack)
    {
        itemIcon.enabled = true;
        quantityText.text = "";
        itemIcon.sprite = stack.itemSO.itemIcon;

        // Only show count when item has more than 1
        quantityText.text = stack.quantity > 1 ? stack.quantity.ToString() : "";
    }
    
    #endregion
}