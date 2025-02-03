using System;
using UnityEngine;
// ������ȫ�ֹ�������Ʒ��
// �����ߣ�Aze
// ����ʱ�䣺2025-01-02
namespace Sprout.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("��Ʒ����")]
        public ItemDataList_SO itemDataList_SO;
        [Header("��������")]
        public InventoryBag_SO playerBag;
        [Header("��������")]
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
            //�Ƿ��п�λ
            var index = GetItemIndexInBag(item.itemID);
            AddItemAtIndex(item.itemID,index,1);
            Debug.Log("ʰȡ��Ʒ");
            if (isDestory)
            {
                Destroy(item.gameObject);
            }

            //����UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);

        }

        /// <summary>
        /// �����Ʒ�Ƿ��п�λ
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
        /// ͨ��ID������Ʒλ��
        /// </summary>
        /// <param name="id">��ƷID</param>
        /// <returns>-1������Ʒ���</returns>
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
        /// ��ָ����Ŵ���ӻ����޸���Ʒ
        /// </summary>
        /// <param name="ID">��ƷID</param>
        /// <param name="index">��Ʒλ��</param>
        /// <param name="amount">��Ʒ����</param>
        private void AddItemAtIndex(int ID,int index,int amount)
        {
            if(index == -1 && CheckBagCapacity())     //����û�е�ǰ��Ʒ
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
            else    //�����е�ǰ��Ʒ
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
        /// �Ƴ�ָ�������ı�����Ʒ
        /// </summary>
        /// <param name="id">��ƷID</param>
        /// <param name="amount">��Ʒ����</param>
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

