// 描述：存储所有的枚举类型。
// 创建者：Aze
// 创建时间：2025-01-02

/// <summary>
/// 物品类型
/// </summary>
public enum ItemType
{
    //种子，商品，家具
    Seed,Commodity,Furniture,
    //挖地工具，砍树工具，浇水工具
    HoeTool,ChopTool,WaterTool,Basket
}
public enum SlotType
{
    Bag,Box,Shop
}

public enum InventoryLocation
{
    Player,Box,Shop
}

public enum Season
{
    春天,夏天,秋天,冬天,
    Spring
}

public enum CursorStates
{
    Normal,Click,Drag
}

public enum GridType
{
    Diggable,DropItem,PlaceFurniture,NPCObstacle
}