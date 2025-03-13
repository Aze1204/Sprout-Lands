using System;
using System.Collections.Generic;
using UnityEngine;

namespace Event
{
    public static class InventoryEvent
    {
        /// <summary>
        /// 更新库存 UI 的事件，参数为库存位置和物品列表
        /// </summary>
        public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;
        public static void CallUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            UpdateInventoryUI?.Invoke(location, list);
        }

        /// <summary>
        /// 在场景中实例化物品的事件，参数为物品 ID 和位置
        /// </summary>
        public static event Action<int, Vector3> InstantiateItemInScene;
        public static void CallInstantiateItemInScene(int id, Vector3 pos)
        {
            InstantiateItemInScene?.Invoke(id, pos);
        }

        /// <summary>
        /// 丢弃物品的事件，参数为物品 ID、位置和物品类型
        /// </summary>
        public static event Action<int, Vector3, ItemType> DropItemEvent;
        public static void CallDropItemEvent(int id, Vector3 pos, ItemType type)
        {
            DropItemEvent?.Invoke(id, pos, type);
        }

        /// <summary>
        /// 种植种子的事件，参数为种子 ID 和地图格子详细信息
        /// </summary>
        public static event Action<int, TileDetails> PlantSeedEvent;
        public static void CallPlantSeedEvent(int id, TileDetails tileDetails)
        {
            PlantSeedEvent?.Invoke(id, tileDetails);
        }

        /// <summary>
        /// 在玩家位置收获物品的事件，参数为物品 ID
        /// </summary>
        public static event Action<int> HarvestAtPlayerPosition;
        public static void CallHarvestAtPlayerPosition(int id)
        {
            HarvestAtPlayerPosition?.Invoke(id);
        }

        /// <summary>
        /// 建造家具的事件，参数为家具 ID 和位置
        /// </summary>
        public static event Action<int, Vector3> BuildFurnitureEvent;
        public static void CallBuildFurnitureEvent(int id, Vector3 pos)
        {
            BuildFurnitureEvent?.Invoke(id, pos);
        }
    }
}