using System.Collections.Generic;

public interface IItemContainer
{
    bool AddItem(ItemSO itemSO, int quantity);
    bool RemoveItem(ItemSO itemSO, int quantity);
    bool HasItem(ItemSO itemSO, int quantity);
    List<ItemStack> GetItems();
}