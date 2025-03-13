using Inventory.Logic;
using Sprout.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Text valueText;
    [SerializeField] private GameObject bottomPart;
    
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

        //ǿ��ˢ��
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Seed => "种子",
            ItemType.WaterTool => "ˮ��",
            ItemType.HoeTool => "��ͷ",
            ItemType.ChopTool => "��ͷ",
            ItemType.Commodity => "��Ʒ",
            ItemType.Furniture => "�Ҿ�",
            _ => "��"
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
