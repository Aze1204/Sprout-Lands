using System;
using System.Collections.Generic;
using Event;
using Inventory.Logic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Sprout.Inventory 
{
    public class ItemManager : MonoBehaviour
    {
        public Item itemPrefab;
        private Transform itemParent;
        private Tilemap tilemap;

        //��¼����Item
        private Dictionary<string,List<SceneItem>> sceneItemDict = new Dictionary<string,List<SceneItem>>();
        private Dictionary<string,List<SceneFurniture>> sceneFurnitureDict = new Dictionary<string,List<SceneFurniture>>();

        private void Awake()
        {
            tilemap = FindObjectOfType<Tilemap>();
        }

        private void OnEnable()
        {
            InventoryEvent.InstantiateItemInScene += OnInstantiateItemInScene;
            InventoryEvent.DropItemEvent += OnDropItemEvent;
            SceneEvent.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            SceneEvent.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
            
            InventoryEvent.BuildFurnitureEvent += OnBuildFurnitureEvent;
        }

        private void OnDisable()
        {
            InventoryEvent.InstantiateItemInScene -= OnInstantiateItemInScene;
            InventoryEvent.DropItemEvent -= OnDropItemEvent;
            SceneEvent.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            SceneEvent.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
            
            InventoryEvent.BuildFurnitureEvent -= OnBuildFurnitureEvent;
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
            Vector3 cellSize = tilemap.cellSize;

            float randomX = Random.Range(-cellSize.x / 2f, cellSize.x / 2f);
            float randomY = Random.Range(-cellSize.y / 2f, cellSize.y / 2f);
            Vector3 randomOffset = new Vector3(randomX, randomY, 0);
            
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
            sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItems;
        }
        
        private void RecreateAllItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();
            if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name,out currentSceneItems))
            {
                if (currentSceneItems != null)
                {
                    //�峡
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
                sceneFurnitureDict[SceneManager.GetActiveScene().name] = currentSceneFurniture;
            }
            else    
            {
                sceneFurnitureDict.Add(SceneManager.GetActiveScene().name, currentSceneFurniture);
            }
        }
        
        private void RebuildFurniture()
        {
            if (sceneFurnitureDict.TryGetValue(SceneManager.GetActiveScene().name, out var currentSceneFurniture))
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