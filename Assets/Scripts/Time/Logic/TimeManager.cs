using Event;
using UnityEngine;
public class TimeManager:MonoBehaviour
{
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;
    private Season season = Season.Spring;
    private int monthInSeason = 1;

    public bool gameClockPause;

    private float tikTime;
    
    private void Awake()
    {
        
        NewGameTime();
    }

    private void Start()
    {
        TimeEvent.CallGameDateEvent(gameHour,gameDay,gameMonth,gameYear,season);
        TimeEvent.CallGameMinuteEvent(gameMinute,gameHour);
    }

    private void Update()
    {
        if (!gameClockPause)
        {
            tikTime += Time.deltaTime;
            if (tikTime>=Settings.secondThreshold)
            {
                tikTime -= Settings.secondThreshold;
                UpdateGameTime();
            }
        }

        if (Input.GetKey(KeyCode.T))
        {
            for (int i=0;i<60;i++)
            {
                UpdateGameTime();
            }
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            gameDay++;
            TimeEvent.CallGameDayEvent(gameDay);
            TimeEvent.CallGameDateEvent(gameHour,gameDay,gameMonth,gameYear,season);
        }

    }

    private void NewGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 6;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 205;
        season = Season.Spring;
    }

    private void UpdateGameTime()
    {
        gameSecond++;
        if (gameSecond > Settings.secondHold)
        {
            gameMinute++;
            gameSecond = 0;

            if (gameMinute > Settings.minuteHold)
            {
                gameHour++;
                gameMinute = 0;

                if (gameHour > Settings.hourHold)
                {
                    gameDay++;
                    gameHour = 0;

                    if (gameDay > Settings.dayHold)
                    {
                        gameDay = 1;
                        gameMonth++;

                        if (gameMonth>1)
                        {
                            gameMonth = 1;
                        }

                        monthInSeason--;
                        if (monthInSeason == 0)
                        {
                            monthInSeason = 1;

                            int seasonNumber = (int)season;
                            seasonNumber++;

                            if (seasonNumber > Settings.seasonHold)
                            {
                                seasonNumber = 0;
                                gameYear++;
                            }
                            season = (Season)seasonNumber;
                        }
                        
                        TimeEvent.CallGameDayEvent(gameDay);
                    }
                }
                TimeEvent.CallGameDateEvent(gameHour,gameDay,gameMonth,gameYear,season);
            }
            TimeEvent.CallGameMinuteEvent(gameMinute,gameHour);
        }
    }
}
