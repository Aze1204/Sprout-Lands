using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
// 描述：存储场景的瓦片信息。
// 创建者：Aze
// 创建时间：2025-01-23
[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    public MapData_SO mapData;
    public GridType gridType;
    private Tilemap currentTilemap;

    private void OnEnable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();
            if (currentTilemap != null)
            {
                mapData.tileProperties.Clear();
            }
        }
    }

    private void OnDisable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();

            UpdateTileProperties();
#if UNITY_EDITOR
            if (mapData!=null)
            {
                EditorUtility.SetDirty(mapData);
            }
#endif
        }
    }

    private void UpdateTileProperties()
    {
        currentTilemap.CompressBounds();
        if (!Application.IsPlaying(this))
        {
            if (mapData!=null)
            {
                //绘制区域的左下角区域
                var startPos = currentTilemap.cellBounds.min;
                //绘制区域的右上角区域
                var endPos = currentTilemap.cellBounds.max;

                for (int x = startPos.x; x<endPos.x; x++)
                {
                    for (int y = startPos.y; y<endPos.y; y++)
                    {
                        TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));
                        if (tile != null)
                        {
                            TileProperty newTile = new TileProperty 
                            {
                                tileCoordinate = new Vector2Int(x, y),
                                gridType = this.gridType,
                                boolTypeValue = true
                            };

                            mapData.tileProperties.Add(newTile);
                        }
                    }
                }
            }
        }
    }
}
