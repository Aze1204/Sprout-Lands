using System;
using UnityEngine;
// ���������������������ϸ��Ϣ��
// �����ߣ�Aze
// ����ʱ�䣺2025-01-01

/// <summary>
/// ��Ʒ��ϸ��Ϣ���൱����Ʒ�������������
/// </summary>
[Serializable]
public class ItemDetails
{
    public int id;
    public string name;
    public ItemType itemType;

    public Sprite itemIcon;
    public Sprite itemOnWorld;      //�����糡���ϵ���Ʒ
    public string itemDescription;

    public int itemUseRadius;       //��Ʒ���÷�Χ
    public bool canPickedup;        //���Ա�ʰȡ
    public bool canDropped;         //���Ա��ӵ�

    public int itemPrice;
    [Range(0,1)]
    public float sellPercentage;    //����ʱ���ۿ۱���
}

/// <summary>
/// ������Ʒ������Ϣ������id������
/// </summary>
[Serializable]
public struct InventoryItem 
{
    public int itemID;
    public int itemAmount;
}

/// <summary>
/// �ɱ����л�������洢
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