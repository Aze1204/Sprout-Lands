using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public static class EventHandler
{
    public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;
    public static void CallUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        UpdateInventoryUI?.Invoke(location, list);
    }

    public static event Action<int, Vector3> InstantiateItemInScene;
    public static void CallInstantiateItemInScene(int id, Vector3 pos)
    {
        InstantiateItemInScene?.Invoke(id, pos);
    }
    
    public static event Action<int, Vector3,ItemType> DropItemEvent;

    public static void CallDropItemEvent(int id, Vector3 pos,ItemType type)
    {
        DropItemEvent?.Invoke(id, pos,type);
    }
    
    public static event Action<int, int> GameMinuteEvent;
    public static void CallGameMinuteEvent(int minute, int hour)
    {
        GameMinuteEvent?.Invoke(minute, hour);
    }

    public static event Action<int> GameDayEvent;
    public static void CallGameDayEvent(int day)
    {
        GameDayEvent?.Invoke(day);
    }
    
    
    public static event System.Action<int, int, int, int, Season> GameDateEvent;
    public static void CallGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        GameDateEvent?.Invoke(hour, day, month, year, season);
    }

    public static event Action<string, Vector3> TransitionEvent;
    public static void CallTransitionEvent(string sceneName, Vector3 pos)
    {
        TransitionEvent.Invoke(sceneName, pos);
    }

    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    public static event Action AfterSceneUnloadEvent;
    public static void CallAfterSceneUnloadEvent()
    {
        AfterSceneUnloadEvent?.Invoke();
    }

    public static event Action<Vector3> MoveToPosition;
    public static void CallMoveToPosition(Vector3 targetPosition)
    {
        MoveToPosition?.Invoke(targetPosition);
    }

    public static event Action<CursorStates,ItemDetails,bool> CursorChangeEvent;
    public static void CallCursorChangeEvent(CursorStates states,ItemDetails itemDetails,bool isSelected)
    {
        CursorChangeEvent?.Invoke(states,itemDetails,isSelected);
    }

    public static event Action<CursorStates, ItemDetails> CursorChangeEventWithSelection;
    public static void CallCursorChangeEventWithSelection(CursorStates states, ItemDetails itemDetails)
    {
        CursorChangeEventWithSelection?.Invoke(states, itemDetails);
    }

    public static event Action<Vector3, ItemDetails> MouseClickedEvent;
    public static void CallMouseClickedEvent(Vector3 pos, ItemDetails itemDetails)
    {
        MouseClickedEvent?.Invoke(pos,itemDetails);
    }
    
    public static event Action<Vector3, ItemDetails> ExecuteActionAfterAnimationEvent;
    public static void CallExecuteActionAfterAnimationEvent(Vector3 pos, ItemDetails itemDetails)
    {
        ExecuteActionAfterAnimationEvent?.Invoke(pos,itemDetails);
    }
    
    public static event Action<int,TileDetails> PlantSeedEvent;

    public static void CallPlantSeedEvent(int id,TileDetails tileDetails)
    {
        PlantSeedEvent?.Invoke(id,tileDetails);
    }

    public static event Action<int> HarvestAtPlayerPosition;
    public static void CallHarvestAtPlayerPosition(int id)
    {
        HarvestAtPlayerPosition?.Invoke(id);
    }
    
    public static event Action<int,Vector3> BuildFurnitureEvent;
    public static void CallBuildFurnitureEvent(int id,Vector3 pos)
    {
        BuildFurnitureEvent?.Invoke(id,pos);
    }
}

