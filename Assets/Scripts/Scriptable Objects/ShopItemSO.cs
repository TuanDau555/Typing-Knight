using UnityEngine;

[CreateAssetMenu(menuName = "Shop/ShopItem")]
public class ShopItemSO : ScriptableObject
{
    [Header("Item")]
    public ItemSO itemSO;

    [Header("Price")]
    public int basePrice;

    [Header("Purchase Rule")]
    [Tooltip("Some items can only buy one time")]
    public bool oneTimePurchase;
}