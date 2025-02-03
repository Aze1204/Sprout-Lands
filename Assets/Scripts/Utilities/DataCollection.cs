using System;
using UnityEngine;
// 描述：包括各种物体的详细信息。
// 创建者：Aze
// 创建时间：2025-01-01

/// <summary>
/// 物品详细信息，相当于物品自身的所有属性
/// </summary>
[Serializable]
public class ItemDetails
{
    public int id;
    public string name;
    public ItemType itemType;

    public Sprite itemIcon;
    public Sprite itemOnWorld;      //在世界场景上的物品
    public string itemDescription;

    public int itemUseRadius;       //物品适用范围
    public bool canPickedup;        //可以被拾取
    public bool canDropped;         //可以被扔掉

    public int itemPrice;
    [Range(0,1)]
    public float sellPercentage;    //出售时的折扣比例
}

/// <summary>
/// 背包物品数据信息，包括id和数量
/// </summary>
[Serializable]
public struct InventoryItem 
{
    public int itemID;
    public int itemAmount;
}

/// <summary>
/// 可被序列化的坐标存储
/// </summary>
public class SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x,y,z);
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x,(int)y);
    }
}

[Serializable]
public class SceneItem
{
    public int itemID;
    public SerializableVector3 position;
}

[Serializable]
public class TileProperty
{
    public Vector2Int tileCoordinate;
    public GridType gridType;

    public bool boolTypeValue;
}

[Serializable]
public class SceneFurniture
{
    public int itemID;
    public SerializableVector3 position;
    public int boxIndex;
}

[Serializable]
public class TileDetails
{
    public int girdX, gridY;
    public bool canDig;
    public bool canDropItem;
    public bool canPlaceFurniture;
    public bool isNPCObstacle;
    public int daySinceDug = -1;
    public int daySinceWatered = -1;
    public int seedItemID = -1;
    public int growthDays = -1;
    public int dyasSinceLastHarvest = -1;
}