using System.Collections.Generic;
using UnityEngine;
// �������־û�������Ʒ���ݡ�
// �����ߣ�Aze
// ����ʱ�䣺2025-01-23
[CreateAssetMenu(fileName = "MapData_SO", menuName = "Map/MapData")]
public class MapData_SO : ScriptableObject
{
    [SceneName]
    public string sceneName;

    public List<TileProperty> tileProperties;
}
