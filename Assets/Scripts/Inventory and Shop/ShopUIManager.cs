using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    #region Parameters

    [Tooltip("This is where item need to spawn in")]
    [SerializeField] private Transform content;
    [SerializeField] private ShopItemUI itemPrefab;

    #endregion

    #region Execute

    private void Start()
    {
        BuildShop();
    }

    #endregion

    #region Build Shop

    private void BuildShop()
    {
        var items = ShopManager.Instance.GetShopItemSO();

        foreach(var item in items)
        {
            var slot = Instantiate(itemPrefab, content);
            slot.Setup(item);
        }
    }
    
    #endregion
}