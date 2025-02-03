using System;
using UnityEngine;
// 描述：全局管理背包物品。
// 创建者：Aze
// 创建时间：2025-01-02
namespace Sprout.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;
        [Header("背包数据")]
        public InventoryBag_SO playerBag;
        [Header("建造数据")]
        public BluePrintDataList_SO bluePrintData;

        private InventoryBag_SO currentBoxBag;
        private void OnEnable()
        {
            EventHandler.DropItemEvent += OnDropItemEvent;
            EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
            EventHandler.BuildFurnitureEvent += OnBuildFurnitureEvent;
        }

        private void OnDisable()
        {
            EventHandler.DropItemEvent -= OnDropItemEvent;
            EventHandler.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
            EventHandler.BuildFurnitureEvent -= OnBuildFurnitureEvent;
        }

        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
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
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
        }
        
        private void OnBuildFurnitureEvent(int id, Vector3 pos)
        {
            //RemoveItem(id, 1);
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
            //是否有空位
            var index = GetItemIndexInBag(item.itemID);
            AddItemAtIndex(item.itemID,index,1);
            Debug.Log("拾取物品");
            if (isDestory)
            {
                Destroy(item.gameObject);
            }

            //更新UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);

        }

        /// <summary>
        /// 检查物品是否有空位
        /// </summary>
        /// <returns>true or false</returns>
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

        /// <summary>
        /// 通过ID查找物品位置
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns>-1——物品序号</returns>
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

        /// <summary>
        /// 在指定序号处添加或者修改物品
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <param name="index">物品位置</param>
        /// <param name="amount">物品数量</param>
        private void AddItemAtIndex(int ID,int index,int amount)
        {
            if(index == -1 && CheckBagCapacity())     //背包没有当前物品
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
            else    //背包有当前物品
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

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
        }

        /// <summary>
        /// 移除指定数量的背包物品
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <param name="amount">物品数量</param>
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
            
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
        }
        
        public bool CheckStock(int ID)
        {
            var bluePrintDetails = bluePrintData.GetBluePrintDetails(ID);

            foreach (var resourceItem in bluePrintDetails.resourceItem)
            {
                var itemStock = playerBag.GetInventoryItem(resourceItem.itemID);
                if (itemStock.itemAmount >= resourceItem.itemAmount)
                {
                    continue;
                }
                else return false;
            }
            return true;
        }
    }
}

