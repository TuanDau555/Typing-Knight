using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    #region Parematers

    [SerializeField] private int maxSlots = 20;
    
    private List<ItemStack> inventory = new List<ItemStack>();

    public event Action OnInventoryChanged;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("InventoryManager Awake");
    }

    #region Add Item

    public void AddItem(ItemSO itemSO, int quantity)
    {
        if(inventory.Count >= maxSlots)
        {
            Debug.Log("Inventory Full");
            return;
        }
        
        while(quantity > 0)
        {
            // Only add items if they are in the database or not exceed the stack limit.
            ItemStack stack = inventory.Find(i => 
            i.itemSO == itemSO 
            && i.quantity < itemSO.maxStack);

            if(stack != null)
            {
                int space = itemSO.maxStack - stack.quantity;
                int addAmount = Mathf.Min(space, quantity);
                
                stack.quantity += addAmount;
                quantity -= addAmount;
            }
            else
            {
                int addAmount = Mathf.Min(itemSO.maxStack, quantity);
                inventory.Add(new ItemStack(itemSO, addAmount));

                quantity -= addAmount;
            }

            Debug.Log($"Added {itemSO.itemName} x {quantity}");
            OnInventoryChanged?.Invoke();
        }
        
    }
    
    #endregion

    #region Get Item

    public List<ItemStack> GetInventory()
    {
        return inventory;
    }
    
    #endregion
}