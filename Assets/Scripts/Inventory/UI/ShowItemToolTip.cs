using UnityEngine;
using UnityEngine.EventSystems;
// 描述：展示物品描述面板。
// 创建者：Aze
// 创建时间：2025-01-11
namespace Sprout.Inventory
{
    [RequireComponent(typeof(SlotUI))]

    public class ShowItemToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private SlotUI slotUI;
        private InventoryUI InventoryUI => GetComponentInParent<InventoryUI>();
        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (slotUI.itemDetails != null)
            {
                InventoryUI.itemToolTip.gameObject.SetActive(true);
                InventoryUI.itemToolTip.SetUpToolTip(slotUI.itemDetails,slotUI.slotType);

                InventoryUI.itemToolTip.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0);
                InventoryUI.itemToolTip.transform.position = transform.position + Vector3.up * 80;
                EventHandler.CallCursorChangeEventWithSelection(CursorStates.Click,slotUI.itemDetails);

                if (slotUI.itemDetails.itemType == ItemType.Furniture)
                {
                    InventoryUI.itemToolTip.resourcePanel.SetActive(true);
                    InventoryUI.itemToolTip.SetupResourcePanel(slotUI.itemDetails.id);
                }
                else
                {
                    InventoryUI.itemToolTip.resourcePanel.SetActive(false);
                }
            }
            else
            {
                InventoryUI.itemToolTip.gameObject.SetActive(false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            InventoryUI.itemToolTip.gameObject.SetActive(false);
            EventHandler.CallCursorChangeEventWithSelection(CursorStates.Normal,slotUI.itemDetails);
        }
    }
}

