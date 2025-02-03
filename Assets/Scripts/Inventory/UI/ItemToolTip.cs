using Sprout.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// 描述：物品详细描述面板。
// 创建者：Aze
// 创建时间：2025-01-11
public class ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Text valueText;
    [SerializeField] private GameObject bottomPart;

    [Header("建造")] 
    public GameObject resourcePanel;
    [SerializeField] private Image[] resourceImages;
    public void SetUpToolTip(ItemDetails itemDetails, SlotType slotType)
    {
        nameText.text = itemDetails.name;
        typeText.text = GetItemType(itemDetails.itemType);
        descriptionText.text = itemDetails.itemDescription;

        if (itemDetails.itemType == ItemType.Seed
            || itemDetails.itemType == ItemType.Commodity
            || itemDetails.itemType == ItemType.Furniture)
        {
            bottomPart.SetActive(true);
            var price = itemDetails.itemPrice;
            if (slotType == SlotType.Bag)
            {
                price = (int)(price * itemDetails.sellPercentage);
            }
            valueText.text = price.ToString();
        }
        else
        {
            bottomPart.SetActive(false);
        }

        //强制刷新
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Seed => "种子",
            ItemType.WaterTool => "水壶",
            ItemType.HoeTool => "锄头",
            ItemType.ChopTool => "斧头",
            ItemType.Commodity => "商品",
            ItemType.Furniture => "家具",
            _ => "无"
        };
    }

    public void SetupResourcePanel(int ID)
    {
        var bluePrintDetails = InventoryManager.Instance.bluePrintData.GetBluePrintDetails(ID);

        for (int i = 0; i < resourceImages.Length; i++)
        {
            if (i < bluePrintDetails.resourceItem.Length)
            {
                var item = bluePrintDetails.resourceItem[i];
                resourceImages[i].gameObject.SetActive(true);
                resourceImages[i].sprite = InventoryManager.Instance.GetItemDetails(item.itemID).itemIcon;
                resourceImages[i].transform.GetChild(0).GetComponent<Text>().text = item.itemAmount.ToString();
            }
            else
            {
                resourceImages[i].gameObject.SetActive(false);
            }
        }
    }

}
