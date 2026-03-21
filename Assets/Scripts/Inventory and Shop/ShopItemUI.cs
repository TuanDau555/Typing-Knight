using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    #region Parameters

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyBtn;
    // [SerializeField] private ItemInfoUI itemInfoUI;
    
    private ShopItemSO shopItemSO;

    #endregion

    #region Start

    private void Start()
    {
        InventoryManager.Instance.OnGoldChanged += UpdateBtn;
    }

    #endregion

    #region Init

    public void Setup(ShopItemSO itemSO)
    {
        shopItemSO = itemSO;

        icon.sprite = itemSO.itemSO.itemIcon;
        priceText.text = itemSO.basePrice.ToString();
        buyBtn.onClick.AddListener(BuyItem);
    }
    
    #endregion

    #region Buy Item

    private void BuyItem()
    {
        ShopManager.Instance.BuyItem(shopItemSO);
    }
    
    private void UpdateBtn()
    {
        int price = ShopManager.Instance.GetPrice(shopItemSO);
        buyBtn.interactable = InventoryManager.Instance.GetGold() >= price;
    }


    #endregion

    #region On Click Item

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(shopItemSO != null)
            ItemInfoUI.Instance.ShowItemInfo(shopItemSO.itemSO);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemInfoUI.Instance.HideItemInfo();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if(shopItemSO != null)
            ItemInfoUI.Instance.FollowMouse();
    }
    
    #endregion
}