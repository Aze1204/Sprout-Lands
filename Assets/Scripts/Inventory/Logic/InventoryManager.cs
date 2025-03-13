using System;
using Event;
using Sprout.Inventory;
using UnityEngine;
using Utilities;

namespace Inventory.Logic
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        public ItemDataList_SO itemDataList_SO;
        public InventoryBag_SO playerBag;
        public BluePrintDataList_SO bluePrintData;

        private InventoryBag_SO currentBoxBag;
        private void OnEnable()
        {
            InventoryEvent.DropItemEvent += OnDropItemEvent;
            InventoryEvent.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
            InventoryEvent.BuildFurnitureEvent += OnBuildFurnitureEvent;
        }

        private void OnDisable()
        {
            InventoryEvent.DropItemEvent -= OnDropItemEvent;
            InventoryEvent.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
            InventoryEvent.BuildFurnitureEvent -= OnBuildFurnitureEvent;
        }

        private void Start()
        {
            InventoryEvent.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
        }
        public ItemDetails GetItemDetails(int id)
        {
            return
                itemDataList_SO.itemDetailsList.Find(x => x.id == id);
        }

        private void OnHarvestAtPlayerPosition(int id)
        {
            var index = GetItemIndexInBag(id);
            AddItemAtIndex(id,index,1);
            InventoryEvent.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
        }
        
        private void OnBuildFurnitureEvent(int id, Vector3 pos)
        {
            BluePrintDetails bluePrintDetails = bluePrintData.GetBluePrintDetails(id);
            foreach (var item in bluePrintDetails.resourceItem)
            {
                RemoveItem(item.itemID,item.itemAmount);
            }
        }
        
        private void OnDropItemEvent(int id, Vector3 pos,ItemType type)
        {
            RemoveItem(id,1);
        }
        public void AddItem(Item item,bool isDestory)
        {
            var index = GetItemIndexInBag(item.itemID);
            AddItemAtIndex(item.itemID,index,1);
            if (isDestory)
            {
                Destroy(item.gameObject);
            }
            
            InventoryEvent.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);

        }
        
        private bool CheckBagCapacity()
        {
            for (int i =0;i<playerBag.itemList.Count;i++)
            {
                if (playerBag.itemList[i].itemID==0)
                {
                    return true;
                }
            }
            return false;
        }
        
        public int GetItemIndexInBag(int id)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == id)
                {
                    return i;
                }
            }
            return -1;
        }
        
        private void AddItemAtIndex(int ID,int index,int amount)
        {
            if(index == -1 && CheckBagCapacity())
            {
                var item = new InventoryItem { itemID = ID ,itemAmount = amount};
                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = item;
                        break;
                    }
                }
            }
            else    
            {
                int currentAmount = playerBag.itemList[index].itemAmount + amount;
                Debug.Log(playerBag.itemList[index].itemID+" "+index);
                var item = new InventoryItem { itemID = ID , itemAmount = currentAmount };
                playerBag.itemList[index] = item;
            } 
        }

        public void SwapItem(int formIndex, int targetIndex)
        {
            InventoryItem currentItem = playerBag.itemList[formIndex];
            InventoryItem targetItem = playerBag.itemList[targetIndex];

            if (targetItem.itemID!=0)
            {
                playerBag.itemList[formIndex] = targetItem;
                playerBag.itemList[targetIndex] = currentItem;
            }
            else
            {
                playerBag.itemList[targetIndex] = currentItem;
                playerBag.itemList[formIndex] = new InventoryItem();
            }

            InventoryEvent.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
        }
        
        private void RemoveItem(int id,int amount)
        {
            var index = GetItemIndexInBag(id);
            if (playerBag.itemList[index].itemAmount>amount)
            {
                var amountToRemove = playerBag.itemList[index].itemAmount - amount;
                var item = new InventoryItem { itemID = id ,itemAmount = amountToRemove};
                playerBag.itemList[index] = item;
            }
            else if(playerBag.itemList[index].itemAmount==amount)
            {
                var item = new InventoryItem();
                playerBag.itemList[index] = item;
            }
            
            InventoryEvent.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
        }
        
        public bool CheckStock(int ID)
        {
            var bluePrintDetails = bluePrintData.GetBluePrintDetails(ID);

            foreach (var resourceItem in bluePrintDetails.resourceItem)
            {
                var itemStock = playerBag.GetInventoryItem(resourceItem.itemID);
                if (itemStock.itemAmount >= resourceItem.itemAmount)
                {
                }
                else return false;
            }
            return true;
        }
    }
}

