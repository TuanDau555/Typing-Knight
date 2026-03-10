using UnityEngine;

[CreateAssetMenu(menuName = "Shop/ItemSO")]
public class ItemSO : ScriptableObject
{
    [Header("Item general")]
    public string itemName;

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
