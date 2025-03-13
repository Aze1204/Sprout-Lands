using System;
using System.Collections.Generic;
using UnityEngine;
namespace Event
{
    public static class TimeEvent
    {
        /// <summary>
        /// 游戏分钟更新事件，参数为分钟和小时
        /// </summary>
        public static event Action<int, int> GameMinuteEvent;
        public static void CallGameMinuteEvent(int minute, int hour)
        {
            GameMinuteEvent?.Invoke(minute, hour);
        }

        /// <summary>
        /// 游戏日期更新事件，参数为小时、天、月、年和季节
        /// </summary>
        public static event Action<int, int, int, int, Season> GameDateEvent;
        public static void CallGameDateEvent(int hour, int day, int month, int year, Season season)
        {
            GameDateEvent?.Invoke(hour, day, month, year, season);
        }

        /// <summary>
        /// 游戏天数更新事件，参数为天数
        /// </summary>
        public static event Action<int> GameDayEvent;
        public static void CallGameDayEvent(int day)
        {
            GameDayEvent?.Invoke(day);
        }
    }
}