using System.Collections.Generic;
using UnityEngine;

public class ItemLookUp : Singleton<ItemLookUp>
{
    
    #region Parameters
    [SerializeField] private List<ItemSO> itemSOs;
    private Dictionary<string, ItemSO> itemLookUp;

    #endregion

    #region Execute

    protected override void Awake()
    {
        base.Awake();

        itemLookUp = new Dictionary<string, ItemSO>();
        foreach(var item in itemSOs)
        {
            itemLookUp[item.itemID] = item;
        }
    }
    
    #endregion

    #region GET

    public ItemSO GetItem(string id)
    {
        if(itemLookUp.TryGetValue(id, out var item))
            return item;
        
        return null;
    }
    
    #endregion
}