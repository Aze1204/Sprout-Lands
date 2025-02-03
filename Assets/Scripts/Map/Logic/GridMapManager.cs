using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

// 描述：全局管理瓦片。
// 创建者：Aze
// 创建时间：2025-01-24

namespace Sprout.Map
{
    public class GridMapManager : Singleton<GridMapManager>
    {
        [Header("种地瓦片切换信息")]
        public TileBase tileBase;
        private Tilemap digitalTilemap;
        
        [Header("地图信息")]
        public List<MapData_SO> mapData;

        private Dictionary<string,TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();
        
        private Grid currentGrid;
        
        private void OnEnable()
        {
            EventHandler.ExecuteActionAfterAnimationEvent += OnExecuteActionAfterAnimationEvent;
            EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
            EventHandler.GameDayEvent += OnGameDayEvent;
        }

        private void OnDisable()
        {
            EventHandler.ExecuteActionAfterAnimationEvent -= OnExecuteActionAfterAnimationEvent;
            EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
            EventHandler.GameDayEvent -= OnGameDayEvent;
        }
        
        private void Start()
        {
            foreach (var map in mapData)
            {
                InitTileDetailsDict(map);
            }
        }
        
        /// <summary>
        /// 执行时机工具或者物品功能
        /// </summary>
        /// <param name="pos">鼠标位置</param>
        /// <param name="details">物品信息</param>
        private void OnExecuteActionAfterAnimationEvent(Vector3 pos, ItemDetails details)
        {
            var mouseGridPos = currentGrid.WorldToCell(pos);
            var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);

            if (currentTile!=null)
            {
                switch (details.itemType)
                {
                    case ItemType.Seed:
                        EventHandler.CallPlantSeedEvent(details.id, currentTile);
                        EventHandler.CallDropItemEvent(details.id,mouseGridPos,ItemType.Seed);
                        break;
                    case ItemType.Commodity:
                        EventHandler.CallDropItemEvent(details.id,mouseGridPos,ItemType.Commodity);
                        break;
                    case ItemType.HoeTool:
                        SetDigGroundTile(currentTile);
                        currentTile.daySinceDug = 0;
                        currentTile.canDig = false;
                        //currentTile.canDropItem = false;
                        //TODO:音效
                        break;
                    case ItemType.Basket:
                        global::Crop currentCrop = GetCropObject(mouseGridPos);
                        //TODO:收割
                        if (currentCrop != null)
                        {
                            currentCrop.ProcessToolAction(details,currentTile);
                        }
                        else
                        {
                            Debug.Log("Crop不存在");
                        }
                        break;
                    case ItemType.Furniture:
                        Debug.Log(details.id);
                        EventHandler.CallBuildFurnitureEvent(details.id,mouseGridPos);
                        break;
                }
                UpdateTileDetails(currentTile);
            }
        }

        private global::Crop GetCropObject(Vector3 mouseWorldPos)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(mouseWorldPos);
            global::Crop crop = null;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<global::Crop>())
                {
                    crop = colliders[i].GetComponent<global::Crop>();
                }
            }
            return crop;
        }
        
        private void OnAfterSceneUnloadEvent()
        {
            currentGrid = FindObjectOfType<Grid>(); 
            digitalTilemap = GameObject.FindWithTag("Dig").GetComponent<Tilemap>();
            DisplayMap(SceneManager.GetActiveScene().name);
        }
        
        /// <summary>
        /// 每天执行一次
        /// </summary>
        /// <param name="day"></param>
        private void OnGameDayEvent(int day)
        {
            foreach (var tile in tileDetailsDict)
            {
                if (tile.Value.daySinceDug>-1)
                {
                    tile.Value.daySinceDug++;
                }
                //超过时间消除挖坑
                if (tile.Value.daySinceDug>5&&tile.Value.seedItemID == -1)
                {
                    tile.Value.daySinceDug = -1;
                    tile.Value.canDig = true;
                    tile.Value.growthDays = -1;
                }

                if (tile.Value.seedItemID!=-1)
                {
                    tile.Value.growthDays++;
                }
                
            }
            RefreshMap();
        }
        
        private void InitTileDetailsDict(MapData_SO mapData)
        {
            foreach (TileProperty tileProperty in mapData.tileProperties)
            {
                TileDetails tileDetails = new TileDetails 
                {
                    girdX = tileProperty.tileCoordinate.x,
                    gridY = tileProperty.tileCoordinate.y
                };
                string key = " X: "+tileDetails.girdX+"-Y: "+tileDetails.gridY+"-"+ mapData.sceneName;

                if (GetTileDetails(key)!=null)
                {
                    tileDetails = GetTileDetails(key);
                }

                switch (tileProperty.gridType)
                {
                    case GridType.Diggable:
                        tileDetails.canDig = tileProperty.boolTypeValue;
                        break;
                    case GridType.DropItem:
                        tileDetails.canDropItem = tileProperty.boolTypeValue;
                        break;
                    case GridType.PlaceFurniture:
                        tileDetails.canPlaceFurniture = tileProperty.boolTypeValue;
                        break;
                    case GridType.NPCObstacle:
                        tileDetails.isNPCObstacle = tileProperty.boolTypeValue;
                        break;
                }

                if (GetTileDetails(key)!=null)
                {
                    tileDetailsDict[key] = tileDetails;
                }
                else
                {
                    tileDetailsDict.Add(key, tileDetails);
                }
            }
        }

        /// <summary>
        /// 根据Key返回瓦片信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private TileDetails GetTileDetails(string key)
        {
            if (tileDetailsDict.ContainsKey(key))
            {
                return tileDetailsDict[key];
            }
            else
            {
                return null;
            }
        }

        public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPos)
        {
            string key = " X: " + mouseGridPos.x + "-Y: " + mouseGridPos.y + "-" + SceneManager.GetActiveScene().name;
            return GetTileDetails(key);
        }

        private void SetDigGroundTile(TileDetails tileDetails)
        {
            Vector3Int pos = new Vector3Int(tileDetails.girdX, tileDetails.gridY, 0);
            if (digitalTilemap != null)
            {
                digitalTilemap.SetTile(pos,tileBase);
            }
        }
        /// <summary>
        /// 更新瓦片信息
        /// </summary>
        /// <param name="tileDetails">瓦片详情</param>
        private void UpdateTileDetails(TileDetails tileDetails)
        {
            string key = " X: " + tileDetails.girdX + "-Y: " + tileDetails.gridY+ "-" + SceneManager.GetActiveScene().name;
            if (tileDetailsDict.ContainsKey(key))
            {
                tileDetailsDict[key] = tileDetails;
            }
            else
            {
                Debug.Log("没有东西");
            }
        }

        /// <summary>
        /// 刷新地图
        /// </summary>
        private void RefreshMap()
        {
            if (digitalTilemap!=null)
            {
                digitalTilemap.ClearAllTiles();
            }

            foreach (var crop in FindObjectsOfType<global::Crop>())
            {
                Destroy(crop.gameObject);
            }
            
            DisplayMap(SceneManager.GetActiveScene().name);
        }
        
        private void DisplayMap(string sceneName)
        {
            foreach (var tile in tileDetailsDict)
            {
                var key = tile.Key;
                var tileDetails = tile.Value;

                if (key.Contains(sceneName))
                {
                    if (tileDetails.daySinceDug > -1)
                    {
                        SetDigGroundTile(tileDetails);
                    }

                    if (tileDetails.seedItemID>-1)
                    {
                        EventHandler.CallPlantSeedEvent(tileDetails.seedItemID,tileDetails);
                    }
                    //TODO:种子
                }
            }
        }
    }
}

