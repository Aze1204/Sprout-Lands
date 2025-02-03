using UnityEngine;

namespace Sprout.Crop
{
    public class CropManager : Singleton<CropManager>
    {
        public CropDataList_SO cropData;
        private Transform cropParent;
        private Grid currentGrid;

        private void OnEnable()
        {
            EventHandler.PlantSeedEvent += OnPlantSeedEvent;
            EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
        }

        private void OnDisable()
        {
            EventHandler.PlantSeedEvent -= OnPlantSeedEvent;
            EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
        }
        
        private void OnPlantSeedEvent(int id, TileDetails tileDetails)
        {
            CropDetails currentCrop = GetCropDetails(id);
            if (currentCrop!=null && tileDetails.seedItemID == -1)  //第一次耕种
            {
                tileDetails.seedItemID = id;
                tileDetails.growthDays = 0;
                DisplayCropPlant(tileDetails, currentCrop);
            }
            else if (tileDetails.seedItemID != -1)  //刷新地图
            {
                DisplayCropPlant(tileDetails, currentCrop);
            }
        }
        
        private void OnAfterSceneUnloadEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
            cropParent = GameObject.FindWithTag("CropParent").transform;
        }

        /// <summary>
        /// 显示农作物
        /// </summary>
        /// <param name="tileDetails">地图信息</param>
        /// <param name="cropDetails">种子信息</param>
        private void DisplayCropPlant(TileDetails tileDetails,CropDetails cropDetails)
        {
            int growthDays = cropDetails.growthDays.Length;
            int currentStage = 0;
            int dayCounter = cropDetails.TotalGrowthDays;

            for (int i = growthDays - 1; i >= 0; i--)
            {
                if (tileDetails.growthDays>=dayCounter)
                {
                    currentStage = i;
                    break;
                }
                dayCounter -= cropDetails.growthDays[i];
            }
            //获取当前阶段的预设体
            GameObject cropPrefab = cropDetails.prefabs[currentStage];
            Sprite cropSprite = cropDetails.sprites[currentStage];

            Vector3 pos = new Vector3(tileDetails.girdX+0.5f,tileDetails.gridY+0.5f,0);
            GameObject cropInstance = Instantiate(cropPrefab, pos, Quaternion.identity, cropParent);
            cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = cropSprite;
            
            cropInstance.GetComponent<global::Crop>().cropDetails = cropDetails;
        }
        
        public CropDetails GetCropDetails(int id)
        {
            return cropData.cropDetailsList.Find(c=>c.seedItemID == id);
        }
    }
}

