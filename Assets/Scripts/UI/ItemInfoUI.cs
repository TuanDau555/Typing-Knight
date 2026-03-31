using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoUI : Singleton<ItemInfoUI>
{
    #region Parameters

    [Header("Item Panel")]
    [SerializeField] private CanvasGroup infoPanel;
    
    [Space(10)]
    [Header("Item Info")]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [Space(10)]
    [Header("Item Stats")]
    [SerializeField] private TextMeshProUGUI[] statText;

    private RectTransform infoPanelRect;

    #endregion

    #region Execute

    protected override void Awake()
    {
        base.Awake();
        infoPanelRect = GetComponent<RectTransform>();
    }

    #endregion

    #region Show/Hide Info

    public void ShowItemInfo(ItemSO itemSO)
    {
        infoPanel.alpha = 1;

        itemNameText.text = itemSO.itemName;
        itemDescription.text = itemSO.description;

        List<string> stats = new List<string>();
        if(itemSO.health > 0) stats.Add($"Health +{itemSO.health.ToString()}");
        if(itemSO.damage > 0) stats.Add($"Damage +{itemSO.damage.ToString()}");
        if(itemSO.speed > 0) stats.Add($"Speed +{itemSO.speed.ToString()}");
        if(itemSO.duration > 0) stats.Add($"Duration ~{itemSO.duration.ToString()}");

        if(stats.Count <= 0) return;

        for(int i = 0; i < statText.Length; i++)
        {
            if(i < stats.Count)
            {
                statText[i].text = stats[i];
                statText[i].gameObject.SetActive(true);
            }
            else
            {
                statText[i].gameObject.SetActive(false);
            }
        }

    }

    public void HideItemInfo()
    {
        infoPanel.alpha = 0;

        itemNameText.text = "";
        itemDescription.text = "";
        
    }

    public void FollowMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 offset = new Vector3(10, -10, 0);

        infoPanelRect.position = mousePosition + offset;
    }
    
    #endregion
}