
using System;
using UnityEngine;

namespace Event
{
    public static class SceneEvent
    {
        /// <summary>
        /// 场景卸载前触发的事件
        /// </summary>
        public static event Action BeforeSceneUnloadEvent;
        public static void CallBeforeSceneUnloadEvent()
        {
            BeforeSceneUnloadEvent?.Invoke();
        }

        /// <summary>
        /// 场景卸载后触发的事件
        /// </summary>
        public static event Action AfterSceneUnloadEvent;
        public static void CallAfterSceneUnloadEvent()
        {
            AfterSceneUnloadEvent?.Invoke();
        }

        /// <summary>
        /// 场景切换事件，参数为目标场景名称和目标位置
        /// </summary>
        public static event Action<string, Vector3> TransitionEvent;
        public static void CallTransitionEvent(string sceneName, Vector3 pos)
        {
            TransitionEvent?.Invoke(sceneName, pos);
        }

        /// <summary>
        /// 移动到指定位置的事件，参数为目标位置
        /// </summary>
        public static event Action<Vector3> MoveToPosition;
        public static void CallMoveToPosition(Vector3 targetPosition)
        {
            MoveToPosition?.Invoke(targetPosition);
        }
    }
}