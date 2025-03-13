using UnityEngine;
using Map.Logic;
using Sprout.Crop;

namespace Cursor
{
    public class CursorValidation : MonoBehaviour
    {
        private Grid currentGrid;
        private Transform playerTransform;

        public void Initialize(Grid grid, Transform player)
        {
            currentGrid = grid;
            playerTransform = player;
        }

        public bool ValidateCursorPosition(Vector3 mouseWorldPos, ItemDetails currentItem, out Vector3Int mouseGridPos)
        {
            mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
            var playerGridPos = currentGrid.WorldToCell(playerTransform.position);

            // 距离检查
            if (Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currentItem.itemUseRadius ||
                Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currentItem.itemUseRadius)
            {
                return false;
            }

            // 地块和作物检查
            TileDetails currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);
            if (currentTile == null) return true;

            // 根据道具类型验证
            return currentItem.itemType switch
            {
                ItemType.Seed => currentTile.daySinceDug > -1 && currentTile.seedItemID == -1,
                ItemType.Commodity => currentTile.canDropItem,
                ItemType.HoeTool => currentTile.canDig,
                ItemType.Basket => CheckBasketValid(currentTile, currentItem),
                ItemType.Furniture => true,
                _ => false
            };
        }

        private bool CheckBasketValid(TileDetails tile, ItemDetails item)
        {
            CropDetails crop = CropManager.Instance.GetCropDetails(tile.seedItemID);
            return crop != null && crop.CheckToolAvailable(item.id) && tile.growthDays >= crop.TotalGrowthDays;
        }
    }
}