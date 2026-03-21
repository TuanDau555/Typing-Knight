using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/ItemSO")]
public class ItemSO : ScriptableObject
{
    [Header("Save Id")]
    public string itemID;
    
    [Space(10)]
    [Header("Item general")]
    public string itemName;

    [Space(10)]
    [Header("Sell Price")]
    [Range(1, 100)]
    public int sellPrice;
    
    [Tooltip("Limit of the item")]
    [Range(1, 999)]
    public int maxStack = 1;

    [TextArea(3, 10)]
    public string description;
    public Sprite itemIcon;

    [Tooltip("Inventory will handle gold difference from other items, so this will notify it's gold or normal item")]
    public bool isGold;

    [Space(10)]
    [Header("Stats")]
    public bool usable;

    [Range(0, 10)]
    public float speed;
    
    [Range(0, 50)]
    public float damage;

    [Range(0, 100)]
    public float health;

    [Space(10)]
    [Header("Temporary Items")]

    [Range(0, 10)]
    public float duration;
}

/// <summary>
/// Data class for inventory save
/// </summary>
[Serializable]
public class InventoryItemSave
{
    public string itemID;
    public int quantity;
}
