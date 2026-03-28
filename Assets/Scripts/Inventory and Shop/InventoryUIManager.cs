using System;
using TMPro;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    #region Parameters

    [Tooltip("Attach the slot object")]
    [SerializeField] private InventorySlotUI[] slots;
    
    [SerializeField] private TextMeshProUGUI goldText;
    
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
        InventoryManager.Instance.OnGoldChanged += UpdateGold;
        RefreshUI();
    }


    private void OnDisable()
    {
        InventoryManager.Instance.OnInventoryChanged -= RefreshUI;
        InventoryManager.Instance.OnGoldChanged -= UpdateGold;
    }

    #endregion

    #region Refresh UI

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

    private void UpdateGold()
    {
        goldText.text = InventoryManager.Instance.GetGold().ToString() + "$";
    }

    #endregion 
}