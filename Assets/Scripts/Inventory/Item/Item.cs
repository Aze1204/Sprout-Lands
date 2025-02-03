using UnityEngine;
// 描述：物品相关逻辑。
// 创建者：Aze
// 创建时间：2025-01-02
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

            //获得当前数据
            itemDetails = InventoryManager.Instance.GetItemDetails(itemID);

            if (itemDetails!=null)
            {
                spriteRenderer.sprite = 
                    itemDetails.itemOnWorld ? itemDetails.itemOnWorld : itemDetails.itemIcon;

                //修改碰撞体尺寸
                Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);    //物体的实际尺寸
                coll.size = newSize;
                coll.offset = new Vector2(0,spriteRenderer.sprite.bounds.center.y);
            }
        }
    }
}

