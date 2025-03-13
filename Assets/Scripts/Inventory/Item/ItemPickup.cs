using Inventory.Logic;
using Sprout.Inventory;
using UnityEngine;
// ��������Ʒʰȡ���ܡ�
// �����ߣ�Aze
// ����ʱ�䣺2025-01-02
public class ItemPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            if (item.itemDetails.canPickedup)
            {
                //ʰȡ��Ʒ
                InventoryManager.Instance.AddItem(item,true);
            }
        }
    }
}
