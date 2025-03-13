using Inventory.Logic;
using UnityEngine;
// ��������Ʒ����߼���
// �����ߣ�Aze
// ����ʱ�䣺2025-01-02
namespace Sprout.Inventory
{
    public class Item : MonoBehaviour
    {
        public int itemID;
        private SpriteRenderer spriteRenderer;
        public ItemDetails itemDetails;
        private BoxCollider2D coll;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            coll = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            Init(itemID);
        }

        public void Init(int id)
        {
            itemID = id;

            //��õ�ǰ����
            itemDetails = InventoryManager.Instance.GetItemDetails(itemID);

            if (itemDetails!=null)
            {
                spriteRenderer.sprite = 
                    itemDetails.itemOnWorld ? itemDetails.itemOnWorld : itemDetails.itemIcon;

                //�޸���ײ��ߴ�
                Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);    //�����ʵ�ʳߴ�
                coll.size = newSize;
                coll.offset = new Vector2(0,spriteRenderer.sprite.bounds.center.y);
            }
        }
    }
}

