using System;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    #region Parameters

    [Tooltip("Attach the slot object")]
    [SerializeField] private InventorySlotUI[] slots;

    #endregion

    #region Execute

    private void Start()
    {
        Debug.Log("InventoryUIManager OnEnable");
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager not found!");
            return;
        }
        Debug.Log("InventoryUIManager OnEnable");
        InventoryManager.Instance.OnInventoryChanged += RefreshUI;
        RefreshUI();
    }


    private void OnDisable()
    {
        InventoryManager.Instance.OnInventoryChanged -= RefreshUI;
    }

    #endregion

    private void RefreshUI()
    {
        var items = InventoryManager.Instance.GetInventory();

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
}