using System.Collections.Generic;
using UnityEngine;
// 描述：持久化场景物品数据。
// 创建者：Aze
// 创建时间：2025-01-23
[CreateAssetMenu(fileName = "MapData_SO", menuName = "Map/MapData")]
public class MapData_SO : ScriptableObject
{
    [SceneName]
    public string sceneName;

    public List<TileProperty> tileProperties;
}
