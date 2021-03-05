using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RealTimeHandler : MonoBehaviour
{
    [SerializeField] int resetHour = 5;

    DateTime resetTime;

    DailyReward dailyReward;
    QuestManager questManager;

    private void Awake() 
    {
        dailyReward = FindObjectOfType<DailyReward>();    
        questManager = FindObjectOfType<QuestManager>();
    }

    private void Start() 
    {
        LoadResetTime();
    }

    private void Update() 
    {
        if(HasPassedResetTime())
        {
            dailyReward.ResetDailyReward();
            questManager.ResetQuestsHasCleared();

            if(DateTime.Now.Hour < resetHour)
            {
                resetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, resetHour, 0, 0);
            }
            else
            {
                resetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, resetHour, 0, 0);
            }           
        }    
    }

    bool HasPassedResetTime()
    {
        if(DateTime.Now >= resetTime) return true;
        else return false;
    }

    public void SaveResetTime()
    {
        ES3.Save<DateTime>("ResetTime", resetTime);
    }

    void LoadResetTime()
    {
        if(ES3.KeyExists("ResetTime"))
        {
            resetTime = ES3.Load<DateTime>("ResetTime");
        }
    }
}
