using Event;
using Inventory.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sprout.Inventory
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        public GameObject slotHighLight;
        [SerializeField] private Button button;
        public SlotType slotType;
        public bool isSelected;
        public int slotIndex;

        public ItemDetails itemDetails;
        public int itemAmount;

        public InventoryUI InventoryUI => GetComponentInParent<InventoryUI>();

        private void Start()
        {
            isSelected = false;
            if (itemDetails == null)
            {
                UpdateEmptySlot();
            }
        }
        
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            amountText.text = amount.ToString();
            slotImage.enabled = true;
            button.interactable = true;
        }
        
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
                InventoryUI.UpdateSlotHightLight(-1);
            }
            itemDetails = null;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
            itemAmount = 0;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemDetails == null)
            {
                return;
            }
            isSelected = !isSelected;
            InventoryUI.UpdateSlotHightLight(slotIndex);
            CursorEvent.CallCursorChangeEvent(CursorStates.Click,itemDetails, isSelected);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount !=0)
            {
                InventoryUI.dragItem.enabled = true;
                InventoryUI.dragItem.sprite = slotImage.sprite;
                isSelected = true;
                InventoryUI.UpdateSlotHightLight(slotIndex);
                CursorEvent.CallCursorChangeEvent(CursorStates.Drag, itemDetails, isSelected);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            InventoryUI.dragItem.transform.position = Input.mousePosition;
            CursorEvent.CallCursorChangeEvent(CursorStates.Drag, itemDetails, isSelected);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            InventoryUI.dragItem.enabled = false;

            if (eventData.pointerCurrentRaycast.gameObject!=null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
                {
                    return;
                }
                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                int targetIndex = targetSlot.slotIndex;
                
                if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    InventoryManager.Instance.SwapItem(slotIndex,targetIndex);
                }
                
                InventoryUI.UpdateSlotHightLight(-1);
            }
            else
            {
                if (itemDetails.canDropped)
                {
                    var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                    InventoryEvent.CallInstantiateItemInScene(itemDetails.id, pos);
                    int currentItemID = InventoryManager.Instance.GetItemIndexInBag(itemDetails.id);
                    InventoryItem newItem = InventoryManager.Instance.playerBag.itemList[currentItemID];
                    newItem.itemID = itemDetails.id;
                    Debug.Log(newItem.itemID);
                    newItem.itemAmount -= 1;
                    InventoryManager.Instance.playerBag.itemList[currentItemID] = newItem;
                    InventoryEvent.CallUpdateInventoryUI(InventoryLocation.Player, InventoryManager.Instance.playerBag.itemList);
                }
            }
            CursorEvent.CallCursorChangeEvent(CursorStates.Normal, itemDetails, false);
        }
    }
}

