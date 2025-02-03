using System;
using UnityEngine;
[Serializable]
public class CropDetails
{
    public int seedItemID;
    [Header("不同阶段需要的天数")] public int[] growthDays;
    public int TotalGrowthDays
    {
        get
        {
            int amount = 0;
            foreach (var days in growthDays)
            {
                amount += days;
            }
            return amount;
        }
    }
    [Header("不同生长阶段物品的Prefab")] public GameObject[] prefabs;
    [Header("不同阶段的图片")] public Sprite[] sprites;

    [Space]
    [Header("收割工具")] public int harvestToolItemID;

    [Header("工具的使用次数")] public int requireActionCount;
    
    [Header("转换新物品ID")] public int transferItemID;
    [Space]
    
    [Header("收割果实信息")] 
    public int[] produceItemID;
    public int[] produceMinAmount;
    public int[] produceMaxAmount;
    public Vector2 spawnRadius;

    [Header("再次生长时间")] 
    public int daysToRegrow;
    public int regrowthDays;
    
    [Header("Options")]
    public bool generateAtPlayerPosition;
    public bool hasAnimation;

    public bool CheckToolAvailable(int toolID)
    {
        if (toolID == harvestToolItemID)
        {
            return true;
        }
        return false;
    }

    public int GetTotalRequireCount(int toolID)
    {
        if (harvestToolItemID == toolID)
        {
            return requireActionCount;
        }
        return -1;
    }
    
}
