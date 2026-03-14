using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>, IItemContainer
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

    #region IItemContainer Item

    public bool AddItem(ItemSO itemSO, int quantity)
    {
        if(inventory.Count >= maxSlots)
        {
            Debug.Log("Inventory Full");
            return false;
        }

        int remaining = quantity;
        
        while(remaining > 0)
        {
            // Only add items if they are in the database or not exceed the stack limit.
            ItemStack stack = inventory.Find(i => 
            i.itemSO == itemSO 
            && i.quantity < itemSO.maxStack);

            if(stack != null)
            {
                int space = itemSO.maxStack - stack.quantity;
                int addAmount = Mathf.Min(space, remaining);
                
                stack.quantity += addAmount;
                remaining -= addAmount;
            }
            else
            {
                if(inventory.Count >= maxSlots)
                {
                    Debug.Log("Inventory Full");
                    OnInventoryChanged?.Invoke();
                    return false;
                }
                
                int addAmount = Mathf.Min(itemSO.maxStack, remaining);
                inventory.Add(new ItemStack(itemSO, addAmount));

                remaining -= addAmount;
            }

        }
        Debug.Log($"Added {itemSO.itemName} x {quantity}");
        OnInventoryChanged?.Invoke();
        return true;
        
    }

    public bool RemoveItem(ItemSO itemSO, int quantity)
    {
        ItemStack stack = inventory.Find(i => i.itemSO == itemSO);
        if(stack == null || stack.quantity < quantity)
        {
            Debug.Log("Not enough items to remove");
            return false;
        }

        stack.quantity -= quantity;
        if(stack.quantity <= 0)
        {
            inventory.Remove(stack);
        }

        Debug.Log($"Removed {itemSO.itemName} x {quantity}");
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool HasItem(ItemSO itemSO, int quantity)
    {
        ItemStack stack = inventory.Find(i => i.itemSO == itemSO);
        return stack != null && stack.quantity >= quantity;
    }

    public List<ItemStack> GetItems()
    {
        return inventory;
    }

    #endregion
}