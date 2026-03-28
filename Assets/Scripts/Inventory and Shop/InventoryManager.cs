using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>, IItemContainer, ISaveable
{
    #region Save Keys
    
    private const string INVENTORY_ITEMS = "Inventory_items";
    private const string INVENTORY_GOLD = "Inventory_gold";

    #endregion
    
    #region Parameters

    [SerializeField] private int maxSlots = 20;
    [SerializeField] private int gold;
    
    private List<ItemStack> inventory = new List<ItemStack>();

    public event Action OnInventoryChanged;
    public event Action OnGoldChanged;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("InventoryManager Awake");
    }

    #region Add/Remove Item

    public bool AddItem(ItemSO itemSO, int quantity)
    {
        // if the item is gold 
        // just increase the text value
        if (itemSO.isGold)
        {
            gold += quantity;
            Debug.Log($"Gold + {quantity} (Total: {gold})");

            OnGoldChanged?.Invoke();
            return true;
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

    #endregion

    #region Spend Gold

    public bool SpendGold(int amount)
    {
        if(gold < amount)
        {
            Debug.Log("Not Enough Gold");
            return false;
        }

        gold -= amount;
        OnGoldChanged?.Invoke();
        
        return true;
    }

    public bool SellItem(ItemSO itemSO)
    {
        gold += itemSO.sellPrice;
        OnGoldChanged?.Invoke();
        return true;
    }
    
    #endregion
    
    #region HasItem
    
    public bool HasItem(ItemSO itemSO, int quantity)
    {
        ItemStack stack = inventory.Find(i => i.itemSO == itemSO);
        return stack != null && stack.quantity >= quantity;
    }


    #endregion

    #region GET

    public List<ItemStack> GetItems()
    {
        return inventory;
    }
    
    public int GetGold()
    {
        return gold;
    }

    #endregion

    #region Save/Load

    public void Save(SaveData data)
    {
        var wrapper = new InventoryItemSaveList();

        foreach(var stack in inventory)
        {
            wrapper.itemSaves.Add(new InventoryItemSave
            {
                itemID = stack.itemSO.itemID,
                quantity = stack.quantity
            });
        }

        data.Set(INVENTORY_ITEMS, wrapper);
        data.Set(INVENTORY_GOLD, gold);
    }

    public void Load(SaveData data)
    {
        inventory.Clear();
        
        var wrapper = data.Get(INVENTORY_ITEMS, new InventoryItemSaveList());
        gold = data.Get(INVENTORY_GOLD, 0);

        foreach(var entry in wrapper.itemSaves)
        {
            ItemSO itemSO = ItemLookUp.Instance.GetItem(entry.itemID);

            if(itemSO != null)
            {
                inventory.Add(new ItemStack(itemSO, entry.quantity));
            }
        }

        OnInventoryChanged?.Invoke();
        OnGoldChanged?.Invoke();
    }

    
    #endregion
}

/// <summary>
/// Wrapper to change List into json file
/// </summary>
[Serializable]
public class InventoryItemSaveList
{
    public List<InventoryItemSave> itemSaves = new List<InventoryItemSave>();
}