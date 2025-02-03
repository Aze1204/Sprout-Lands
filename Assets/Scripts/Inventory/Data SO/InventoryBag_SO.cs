using System.Collections.Generic;
using UnityEngine;
// �������־û�������Ʒ���ݡ�
// �����ߣ�Aze
// ����ʱ�䣺2025-01-02
[CreateAssetMenu(fileName = "InventoryBag_SO", menuName = "Inventory/InventoryBag")]
public class InventoryBag_SO : ScriptableObject
{
    public List<InventoryItem> itemList;

    public InventoryItem GetInventoryItem(int id)
    {
        return itemList.Find(i=>i.itemID==id);
    }
}
