using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    #region Parameters

    [Header("Shop Data")]
    [SerializeField] private ShopDatabaseSO shopDatabaseSO;
    
    private HashSet<ItemSO> purchasedItems = new HashSet<ItemSO>();

    #endregion

    #region Execute

    protected override void Awake()
    {
        base.Awake();
    }
    
    #endregion

    #region Buy Items

    public bool IsPurchase(ItemSO itemSO)
    {
        return purchasedItems.Contains(itemSO);
    }

    public bool BuyItem(ShopItemSO shopItemSO)
    {
        if(shopItemSO.oneTimePurchase && IsPurchase(shopItemSO.itemSO))
        {
            return false;
        }

        int price = GetPrice(shopItemSO);

        if (!InventoryManager.Instance.SpendGold(price))
        {
            Debug.Log("Not enough gold");
            return false;
        }

        // player only buy one item right ?
        // If they want more they can continue buying
        InventoryManager.Instance.AddItem(shopItemSO.itemSO, 1); 

        if (shopItemSO.oneTimePurchase)
        {
            purchasedItems.Add(shopItemSO.itemSO);
        }

        Debug.Log($"Bought {shopItemSO.itemSO.itemName}");
        
        return true;
    }
        
    #endregion
    
    #region Get

    public List<ShopItemSO> GetShopItemSO()
    {
        return shopDatabaseSO.items;
    }

    public int GetPrice(ShopItemSO shopItemSO)
    {
        int playerLevel = 1; // TODO: We need thing to handle this dynamic

        return shopItemSO.basePrice * playerLevel;
    }
    
    #endregion
}
