using Sprout.Inventory;
using UnityEngine;
// 描述：物品拾取功能。
// 创建者：Aze
// 创建时间：2025-01-02
public class ItemPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            if (item.itemDetails.canPickedup)
            {
                //拾取物品
                InventoryManager.Instance.AddItem(item,true);
            }
        }
    }
}
