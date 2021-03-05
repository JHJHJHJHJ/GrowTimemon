using UnityEngine;
using System;
using System.Collections;

public class QuestResultManager : MonoBehaviour 
{
    [Header("Configs")]
    [SerializeField] float accuracyStandard = 0.8f;

    [Header("Data")]
    [SerializeField] int[] questCompleteTimeDiffrences;

    [Header("References")]
    [SerializeField] QuestResultWindow resultWindow = null;
    
    Quest currentQuest = null;
    DateTime[] questTime = new DateTime[2];

    float totalTime;
    float totalDeltaTime;
    int[] rewards = new int[2];

    public void SetCurrentQuest(Quest _quest)
    {
        currentQuest = _quest;
    }

    public void SetCurrentSubquestCompleteTimeDiffrence(int _currentSubquestIndex)
    {
        Timer timer = FindObjectOfType<Timer>();
        SubQuest currentSubquest = currentQuest.subQuestList[_currentSubquestIndex];

        float deltaTime = currentSubquest.second - timer.GetLeftTime();
        float completeTimeDifference = (deltaTime - currentSubquest.second);

        currentQuest.subQuestList[_currentSubquestIndex].SetCompleteTimeDifference(completeTimeDifference);
    }

    public void OpenResultWindow(Quest _currentQuest, DateTime[] _dateTime)
    {
        currentQuest = _currentQuest;
        questTime = _dateTime;

        CalculateClearTime();

        ShowResult();
    }

    void ShowResult()
    {
        resultWindow.gameObject.SetActive(true);
        resultWindow.InitializeWindow();
        
        resultWindow.UpdateTop(currentQuest);

        resultWindow.UpdateClearTime(currentQuest, totalDeltaTime, totalTime, questTime);

        resultWindow.SetUpAdditionalGoals(currentQuest, questTime[0], totalDeltaTime, totalTime, accuracyStandard);
    
        CalculateRewards();
        resultWindow.UpdateRewards(rewards[0], rewards[1]);

        resultWindow.ActivateRewardButton();

        FindObjectOfType<ColorManager>().ChangeColors();
    }

    void CalculateClearTime()
    {
        totalTime = 0;
        totalDeltaTime = 0;

        foreach(SubQuest subquest in currentQuest.subQuestList)
        {
            if(subquest.isTimer)
            {
                totalTime += subquest.second;
                totalDeltaTime += subquest.second + subquest.GetCompleteTimeDifference();
            }  
        }
    }

    void CalculateRewards()
    {
        if(currentQuest.GetHasCleard())
        {
            rewards[0] = 0;
            rewards[1] = 0;
        }
        else
        {
            float gold = currentQuest.rewardGoldAmount;
            float dia = currentQuest.rewardDiaAmount;

            if(resultWindow.additionalGoals[0].isAchieved) 
            {
                dia += 50f;
            }
            if(resultWindow.additionalGoals[1].isAchieved) 
            {
                gold *= 1.2f;
            }
            if(resultWindow.additionalGoals[2].isAchieved) 
            {
                gold *= 1.2f;
            }

            rewards[0] = (int)gold;
            rewards[1] = (int)dia;
        }
    }

    public void GetRewards()
    {
        currentQuest.SetHasCleard(true);

        FindObjectOfType<ResourceManager>().TakeReward(rewards[0], rewards[1]);
        CloseResultWindow();
    }

    public void CloseResultWindow()
    {
        resultWindow.gameObject.SetActive(false);
    }

    public void OpenLogWindow() // 버튼에서 실행됨
    {
        resultWindow.OpenLogWindow(currentQuest);
        FindObjectOfType<ColorManager>().ChangeColors();
    }
}