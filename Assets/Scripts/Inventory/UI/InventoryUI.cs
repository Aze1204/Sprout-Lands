using System.Collections.Generic;
using Event;
using Inventory.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Sprout.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public ItemToolTip itemToolTip;
        
        public Image dragItem;

        [SerializeField] private SlotUI[] playerSlots;
        [SerializeField] private GameObject playerBag;
        [SerializeField] private GameObject box;

        private bool bagOpened;
        private bool boxOpened;

        private void OnEnable()
        {
            InventoryEvent.UpdateInventoryUI += OnUpdateInventoryUI;
        }

        private void OnDisable()
        {
            InventoryEvent.UpdateInventoryUI -= OnUpdateInventoryUI;
        }
        
        private void Start()
        {
            for(int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex = i;
            }
            bagOpened = playerBag.activeInHierarchy;
            boxOpened = box.activeInHierarchy;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                OpenBag();
            }
            if (Input.GetMouseButtonUp(1))
            {
                OpenBox();
            }
        }

        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            switch (location)
            {
                case InventoryLocation.Player:
                    for (int i = 0;i<playerSlots.Length;i++)
                    {
                        if (list[i].itemAmount>0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            playerSlots[i].UpdateSlot(item, list[i].itemAmount);
                        }
                        else
                        {
                            playerSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
            }
        }

        public void OpenBox()
        {
            boxOpened = !boxOpened;
            box.SetActive(boxOpened);
        }
        
        public void OpenBag()
        {
            bagOpened = !bagOpened;
            playerBag.SetActive(bagOpened);
        }
        
        public void UpdateSlotHightLight(int index)
        {
            foreach (var slot in playerSlots)
            {
                if (slot.isSelected && slot.slotIndex == index)
                {
                    slot.slotHighLight.SetActive(true);
                }
                else
                {
                    slot.isSelected = false;
                    slot.slotHighLight.SetActive(false);
                }
            }
        }
    }
}

