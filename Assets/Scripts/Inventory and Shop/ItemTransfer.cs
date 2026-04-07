
public static class ItemTransfer
{
    /// <summary>
    /// This method is call when we want to transfer item from this object to another object
    /// NOTE: this just use for shop, invetory, chest,... 
    /// </summary>
    /// <param name="from">Origin object</param>
    /// <param name="to">Object want to transfer</param>
    /// <returns>Able to transfer object</returns>
    public static bool Transfer(IItemContainer from, IItemContainer to, ItemSO itemSO, int quantity)
    {
        // If this object has item...
        if(!from.HasItem(itemSO, quantity))
            return false;

        // ...transfer to dedicate object
        if(!to.AddItem(itemSO, quantity))
            return false;
        
        // ...and Remove that object from this object
        from.RemoveItem(itemSO, quantity);
        
        return true;
    }
}