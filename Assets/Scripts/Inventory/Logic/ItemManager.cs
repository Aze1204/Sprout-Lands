using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

// 描述：管理场景上的物品。
// 创建者：Aze
// 创建时间：2025-01-02
namespace Sprout.Inventory 
{
    public class ItemManager : MonoBehaviour
    {
        public Item itemPrefab;
        private Transform itemParent;
        private Tilemap tilemap;

        //记录场景Item
        private Dictionary<string,List<SceneItem>> sceneItemDict = new Dictionary<string,List<SceneItem>>();
        private Dictionary<string,List<SceneFurniture>> sceneFurnitureDict = new Dictionary<string,List<SceneFurniture>>();

        private void Awake()
        {
            tilemap = FindObjectOfType<Tilemap>();
        }

        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
            EventHandler.DropItemEvent += OnDropItemEvent;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
            
            //建造
            EventHandler.BuildFurnitureEvent += OnBuildFurnitureEvent;
        }

        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.DropItemEvent -= OnDropItemEvent;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
            
            EventHandler.BuildFurnitureEvent -= OnBuildFurnitureEvent;
        }
        
        private void OnBuildFurnitureEvent(int id,Vector3 pos)
        {
            BluePrintDetails bluePrint = InventoryManager.Instance.bluePrintData.GetBluePrintDetails(id);
            var buildItem = Instantiate(bluePrint.buildPrefab, pos, Quaternion.identity, itemParent);
        }

        private void OnInstantiateItemInScene(int id, Vector3 pos)
        {
            var item = Instantiate(itemPrefab,pos,Quaternion.identity,itemParent);
            item.itemID = id;
        }
        
        private void OnDropItemEvent(int id, Vector3 pos,ItemType itemType)
        {
            if (itemType == ItemType.Seed)
            {
                return;
            }
            // 获取Tilemap的格子尺寸
            Vector3 cellSize = tilemap.cellSize;

            float randomX = Random.Range(-cellSize.x / 2f, cellSize.x / 2f);
            float randomY = Random.Range(-cellSize.y / 2f, cellSize.y / 2f);
            Vector3 randomOffset = new Vector3(randomX, randomY, 0);

            // 在格子内随机位置生成物品
            Vector3 randomPosition = pos + randomOffset;
            var item = Instantiate(itemPrefab,randomPosition,Quaternion.identity,itemParent);
            item.itemID = id;
        }
        
        private void OnBeforeSceneUnloadEvent()
        {
            GetAllSceneItems();
            GetAllSceneFurniture();
        }

        private void OnAfterSceneUnloadEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
            RecreateAllItems();
            RebuildFurniture();
        }

        /// <summary>
        /// 获得当前场景所有Item
        /// </summary>
        private void GetAllSceneItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            foreach (var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem
                {
                    itemID = item.itemID,
                    position = new SerializableVector3(item.transform.position)
                };
                currentSceneItems.Add(sceneItem);
            }
            if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
            {
                sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItems;
            }
            else
            {
                sceneItemDict.Add(SceneManager.GetActiveScene().name,currentSceneItems);
            }
        }

        /// <summary>
        /// 刷新重建所有Item
        /// </summary>
        private void RecreateAllItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();
            if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name,out currentSceneItems))
            {
                if (currentSceneItems != null)
                {
                    //清场
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }

                    foreach (var item in currentSceneItems)
                    {
                        Item newItem = Instantiate(itemPrefab,item.position.ToVector3(),Quaternion.identity,itemParent);
                        newItem.Init(item.itemID);
                    }
                }
            }
        }
        
        /// <summary>
        /// 获得场景所有家具
        /// </summary>
        private void GetAllSceneFurniture()
        {
            List<SceneFurniture> currentSceneFurniture = new List<SceneFurniture>();

            foreach (var item in FindObjectsOfType<Furniture>())
            {
                SceneFurniture sceneFurniture = new SceneFurniture
                {
                    itemID = item.itemID,
                    position = new SerializableVector3(item.transform.position)
                };
                currentSceneFurniture.Add(sceneFurniture);
            }
            if (sceneFurnitureDict.ContainsKey(SceneManager.GetActiveScene().name))
            {
                //找到数据就更新item数据列表
                sceneFurnitureDict[SceneManager.GetActiveScene().name] = currentSceneFurniture;
            }
            else    //如果是新场景
            {
                sceneFurnitureDict.Add(SceneManager.GetActiveScene().name, currentSceneFurniture);
            }
        }
        
        /// <summary>
        /// 重建当前场景家具
        /// </summary>
        private void RebuildFurniture()
        {
            List<SceneFurniture> currentSceneFurniture = new List<SceneFurniture>();

            if (sceneFurnitureDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneFurniture))
            {
                if (currentSceneFurniture != null)
                {
                    foreach (var sceneFurniture in currentSceneFurniture)
                    {
                        BluePrintDetails bluePrint = InventoryManager.Instance.bluePrintData.GetBluePrintDetails(sceneFurniture.itemID);
                        var buildItem = Instantiate(bluePrint.buildPrefab, sceneFurniture.position.ToVector3(), Quaternion.identity, itemParent);
                    }
                }
            }
        }
    }
}