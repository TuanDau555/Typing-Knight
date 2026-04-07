using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    #region Parameters

    [SerializeField] private InventorySlotUI[] slots;

    #endregion

    #region Execute

    private void OnEnable()
    {
        InventoryManager.Instance.OnInventoryChanged += RefreshUI;
    }

    private void OnDisable()
    {
        InventoryManager.Instance.OnInventoryChanged -= RefreshUI;
    }
    
    #endregion

    #region Update UI

    private void RefreshUI()
    {
        var items = InventoryManager.Instance.GetItems();

        for(int i = 0; i < slots.Length; i++)
        {
            if(i < items.Count)
            {
                slots[i].SetItem(items[i]);
            }
            else
            {
                slots[i].SetEmpty();
            }
        }
    }

    #endregion
}