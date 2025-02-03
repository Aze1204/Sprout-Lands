using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Box : MonoBehaviour
{
    public InventoryBag_SO boxBagTemplate;

    public InventoryBag_SO boxBagData;

    private bool canOpen = false;

    private bool isOpen;

    public int index;

    private void OnEnable()
    {
        if (boxBagData == null)
        {
            boxBagData = Instantiate(boxBagTemplate);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canOpen = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canOpen = false;
        }
    }

    private void Update()
    {
        if (!isOpen && canOpen && Input.GetMouseButtonDown(1))
        {
            //打开箱子
            //EventHandler.CallBaseBagOpenEvent(SlotType.Box, boxBagData);
            isOpen = true;
        }

        if (!canOpen && isOpen)
        {
            //关闭箱子
            //EventHandler.CallBaseBagCloseEvent(SlotType.Box, boxBagData);
            isOpen = false;
        }

        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            //关闭箱子
            //EventHandler.CallBaseBagCloseEvent(SlotType.Box, boxBagData);
            isOpen = false;
        }
    }

    public void InitBox(int boxIndex)
    {
        index = boxIndex;
        var key = this.name + index;
        // if (InventoryManager.Instance.GetBoxDataList(key) != null)//刷新地图读取数据
        // {
        //     boxBagData.itemList = InventoryManager.Instance.GetBoxDataList(key);
        // }
        // else//新建箱子
        // {
        //     if (index == 0)
        //         index = InventoryManager.Instance.BoxDataAmount;
        //     InventoryManager.Instance.AddBoxDataDict(this);
        // }
    }
}
    

