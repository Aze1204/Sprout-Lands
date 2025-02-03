using System.Collections.Generic;
using UnityEngine;
// 描述：持久化物品信息。
// 创建者：Aze
// 创建时间：2025-01-02

[CreateAssetMenu(fileName = "ItemDataList_SO",menuName = "Inventory/ItemDataList")]
public class ItemDataList_SO : ScriptableObject
{
    public List<ItemDetails> itemDetailsList;
}
