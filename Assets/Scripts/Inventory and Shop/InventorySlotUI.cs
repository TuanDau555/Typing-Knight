using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This script need to attach to single slot of the inventory
/// </summary>
public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
{
    #region Parameters
    
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI quantityText;
    // [SerializeField] private ItemInfoUI itemInfoUI;

    private ItemStack _itemStack;
    
    #endregion

    #region Set Item

    public void SetEmpty()
    {
        _itemStack = null;
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        quantityText.text = "";
    }

    public void SetItem(ItemStack itemStack)
    {
        _itemStack = itemStack;
        Debug.Log("Item stack: " + _itemStack);
        itemIcon.enabled = true;
        quantityText.text = "";
        itemIcon.sprite = itemStack.itemSO.itemIcon;

        // Only show count when item has more than 1
        quantityText.text = itemStack.quantity > 1 ? itemStack.quantity.ToString() : "";
    }

    
    #endregion

    #region On Item Enter

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_itemStack != null)
            ItemInfoUI.Instance.ShowItemInfo(_itemStack.itemSO);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemInfoUI.Instance.HideItemInfo();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if(_itemStack != null)
            ItemInfoUI.Instance.FollowMouse();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_itemStack != null)
        {
            if(eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log($"Button: {eventData.button}");
                if(SceneManager.GetActiveScene().name == "Main Menu") return;
                UseItem(_itemStack);
            }
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                if(SceneManager.GetActiveScene().name != "Main Menu") return;
                Sell(_itemStack);
            }
        }
    }


    #endregion

    #region On Item Use

    private void UseItem(ItemStack stack)
    {
        if(!stack.itemSO.usable) return;

        // TODO: handle apply item stats to player stats

        ApplyItemEffect(stack.itemSO);

        // Player just use one time
        InventoryManager.Instance.RemoveItem(stack.itemSO, 1);
    }

    private void Sell(ItemStack stack)
    {
        if(stack == null || stack.quantity <= 0)
            return;
        
        ItemSO itemSO = stack.itemSO;
        
        InventoryManager.Instance.SellItem(itemSO);
        InventoryManager.Instance.RemoveItem(itemSO, 1);
    }
    
    private void ApplyItemEffect(ItemSO item)
    {
        Debug.Log($"Applying item: {item.itemName}");

        if(item.health > 0)
            Debug.Log($"Heal +{item.health}");

        if(item.damage > 0)
            Debug.Log($"Damage +{item.damage}");

        if(item.speed > 0)
            Debug.Log($"Speed +{item.speed}");

        if(item.duration > 0)
            Debug.Log($"Duration {item.duration}s");
    }
    
    #endregion
}