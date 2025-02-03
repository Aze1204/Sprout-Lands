using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

// ������ȫ�ֹ�����Ƭ��
// �����ߣ�Aze
// ����ʱ�䣺2025-01-24

namespace Sprout.Map
{
    public class GridMapManager : Singleton<GridMapManager>
    {
        [Header("�ֵ���Ƭ�л���Ϣ")]
        public TileBase tileBase;
        private Tilemap digitalTilemap;
        
        [Header("��ͼ��Ϣ")]
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
        /// ִ��ʱ�����߻�����Ʒ����
        /// </summary>
        /// <param name="pos">���λ��</param>
        /// <param name="details">��Ʒ��Ϣ</param>
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
                        //TODO:��Ч
                        break;
                    case ItemType.Basket:
                        global::Crop currentCrop = GetCropObject(mouseGridPos);
                        //TODO:�ո�
                        if (currentCrop != null)
                        {
                            currentCrop.ProcessToolAction(details,currentTile);
                        }
                        else
                        {
                            Debug.Log("Crop������");
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
        /// ÿ��ִ��һ��
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
                //����ʱ�������ڿ�
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
        /// ����Key������Ƭ��Ϣ
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
        /// ������Ƭ��Ϣ
        /// </summary>
        /// <param name="tileDetails">��Ƭ����</param>
        private void UpdateTileDetails(TileDetails tileDetails)
        {
            string key = " X: " + tileDetails.girdX + "-Y: " + tileDetails.gridY+ "-" + SceneManager.GetActiveScene().name;
            if (tileDetailsDict.ContainsKey(key))
            {
                tileDetailsDict[key] = tileDetails;
            }
            else
            {
                Debug.Log("û�ж���");
            }
        }

        /// <summary>
        /// ˢ�µ�ͼ
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
                    //TODO:����
                }
            }
        }
    }
}

