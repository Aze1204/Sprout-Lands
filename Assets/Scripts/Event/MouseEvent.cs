using System;
using UnityEngine;

namespace Event
{
    public static class MouseEvent
    {
        /// <summary>
        /// 鼠标点击事件，参数为点击位置和物品详细信息
        /// </summary>
        public static event Action<Vector3, ItemDetails> MouseClickedEvent;

        public static void CallMouseClickedEvent(Vector3 pos, ItemDetails itemDetails)
        {
            MouseClickedEvent?.Invoke(pos, itemDetails);
        }

        /// <summary>
        /// 动画执行后执行动作的事件，参数为位置和物品详细信息
        /// </summary>
        public static event Action<Vector3, ItemDetails> ExecuteActionAfterAnimationEvent;

        public static void CallExecuteActionAfterAnimationEvent(Vector3 pos, ItemDetails itemDetails)
        {
            ExecuteActionAfterAnimationEvent?.Invoke(pos, itemDetails);
        }
    }
}