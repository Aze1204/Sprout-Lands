using System.Collections;
using System.Collections.Generic;
using Event;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropDetails cropDetails;
    private TileDetails tileDetails;
    private int harvestActionCount;
    public void ProcessToolAction(ItemDetails details,TileDetails tile)
    {
        tileDetails = tile;
        int requireActionCount = cropDetails.GetTotalRequireCount(details.id);
        if (requireActionCount == -1)
        {
            return;
        }

        if (harvestActionCount<requireActionCount)
        {
            harvestActionCount++;
            //播放声音
        }

        if (harvestActionCount>=requireActionCount)
        {
            if (cropDetails.generateAtPlayerPosition)
            {
                //生成农作物;
                SpawnHarvestItems();
            }
        }
    }

    public void SpawnHarvestItems()
    {
        for (int i = 0; i < cropDetails.produceItemID.Length; i++)
        {
            int amount;
            if (cropDetails.produceMinAmount[i]==cropDetails.produceMaxAmount[i])
            {
                amount = cropDetails.produceMinAmount[i];
            }
            else
            {
                amount = Random.Range(cropDetails.produceMinAmount[i],cropDetails.produceMaxAmount[i]);
            }

            for (int j = 0;j<amount;j++)
            {
                if (cropDetails.generateAtPlayerPosition)
                {
                    InventoryEvent.CallHarvestAtPlayerPosition(cropDetails.produceItemID[i]);
                }
            }
        }

        if (tileDetails!=null)
        {
            tileDetails.dyasSinceLastHarvest++;
            tileDetails.dyasSinceLastHarvest = -1;
            tileDetails.seedItemID = -1;
            Destroy(gameObject);
        }
        
    }
}
