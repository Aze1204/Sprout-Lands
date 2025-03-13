using System;
namespace Event
{
    public static class CursorEvent
    {
        /// <summary>
        /// 光标状态改变事件，参数为光标状态、物品详细信息和是否选中
        /// </summary>
        public static event Action<CursorStates, ItemDetails, bool> CursorChangeEvent;
        public static void CallCursorChangeEvent(CursorStates states, ItemDetails itemDetails, bool isSelected)
        {
            CursorChangeEvent?.Invoke(states, itemDetails, isSelected);
        }

        /// <summary>
        /// 光标状态改变且有选中物品的事件，参数为光标状态和物品详细信息
        /// </summary>
        public static event Action<CursorStates, ItemDetails> CursorChangeEventWithSelection;
        public static void CallCursorChangeEventWithSelection(CursorStates states, ItemDetails itemDetails)
        {
            CursorChangeEventWithSelection?.Invoke(states, itemDetails);
        }
    }
}