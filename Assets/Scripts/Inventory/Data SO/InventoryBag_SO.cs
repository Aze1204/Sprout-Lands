using System.Collections.Generic;
using UnityEngine;
// 描述：持久化背包物品数据。
// 创建者：Aze
// 创建时间：2025-01-02
[CreateAssetMenu(fileName = "InventoryBag_SO", menuName = "Inventory/InventoryBag")]
public class InventoryBag_SO : ScriptableObject
{
    public List<InventoryItem> itemList;

    public InventoryItem GetInventoryItem(int id)
    {
        return itemList.Find(i=>i.itemID==id);
    }
}
