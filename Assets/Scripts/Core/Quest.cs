using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using TMPro;
using System;

public class Quest : MonoBehaviour
{
    [Header("Quest Info")]
    public int id = 0;
    public string title = null;
    public Sprite iconSprite = null;
    public List<SubQuest> subQuestList = new List<SubQuest>();
    public float rewardGoldAmount = 0;
    public float rewardDiaAmount = 0;
    public Alarm alarm;

    bool hasCleared = false;

    [Header("Components")]
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] Image icon = null;
    [SerializeField] GameObject timeLimit = null;
    [SerializeField] ColorChanger timeLimitBackground = null;
    [SerializeField] TextMeshProUGUI timeText = null;
    [SerializeField] GameObject hasClearedMarker = null;

    [HideInInspector] public bool hasClicked = false;

    bool inSwitch = true;
    bool outSwitch = true;

    private void Update() 
    {
        UpdateTimeLimit();    
    }

    public void SetID(int _id)
    {
        id = _id;
    }

    public void SetupQuest(string _title, Sprite _iconSprite, List<SubQuest> _subQuestList, int[] _rewardAmounts, Alarm _alarm)
    {
        title = _title;
        iconSprite = _iconSprite;
        subQuestList = _subQuestList;
        if(_alarm != null)
        {
            alarm = _alarm;
        }

        rewardGoldAmount = _rewardAmounts[0];
        rewardDiaAmount = _rewardAmounts[1];

        SetHasCleard(LoadHasCleared());

        UpdateQuestObject();
    }

    public void UpdateQuestObject()
    {
        titleText.text = title;
        icon.sprite = iconSprite;

        if(alarm != null && alarm.hasAlarm)
        {
            timeLimit.gameObject.SetActive(true);
            timeText.text = alarm.hour + ":" + alarm.minute.ToString("D2") + " " + alarm.noon;
        }
        else
        {
            timeLimit.gameObject.SetActive(false);
        }
    }

    public void StartQuest()
    {
        hasClicked = true;

        InitializeSubquestCompleteTimeDifferences();
    }

    void InitializeSubquestCompleteTimeDifferences()
    {
        foreach (SubQuest subQuest in subQuestList)
        {
            subQuest.SetCompleteTimeDifference(0);
        }
    }

    void UpdateTimeLimit()
    {
        if(alarm == null || !alarm.hasAlarm) return;

        if(IsRightTime(alarm))
        {
            if(inSwitch)
            {
                GetComponent<Animator>().SetBool("IsRightTime", true); 
                timeLimitBackground.ChangeColorValueTo(ColorValue.Mid);
                timeText.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.White);

                inSwitch = false;
                outSwitch = true;
            }
        }
        else
        {
            if(outSwitch)
            {
                GetComponent<Animator>().SetBool("IsRightTime", false);
                timeLimitBackground.ChangeColorValueTo(ColorValue.Light);
                timeText.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.Mid);

                inSwitch = true;
                outSwitch = false;

                print("OUT");
            }
        }
    }

    bool IsRightTime(Alarm _alarm)
    {
        int hour = _alarm.hour;
        if (hour == 12)
        {
            if (_alarm.noon == Noon.AM) hour = 0;
        }
        else if (_alarm.noon == Noon.PM)
        {
            hour += 12;
        }

        DateTime dateTime = new DateTime(
        DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
        hour, _alarm.minute, 0);

        DateTime before10m = dateTime.AddMinutes(-10d);
        DateTime after10m = dateTime.AddMinutes(10d);

        return (before10m <= DateTime.Now && DateTime.Now <= after10m);
    }

    public void SetHasCleard(bool _hasCleared)
    {
        hasCleared = _hasCleared;
        hasClearedMarker.SetActive(_hasCleared);
    }

    public bool GetHasCleard()
    {
        return hasCleared;
    }

    public void SaveHasCleard()
    {
        ES3.Save<bool>("HasCleard_" + id.ToString(), hasCleared);
    }

    bool LoadHasCleared()
    {
        if(ES3.KeyExists("HasCleard_" + id.ToString()))
        {
            return ES3.Load<bool>("HasCleard_" + id.ToString());
        }
        else
        {
            return hasCleared;
        }
    }
}

[System.Serializable]
public class SubQuest
{
    public string title;
    public bool isTimer = false;
    public float second;
    float completeTimeDifference;

    public void SetCompleteTimeDifference(float _value)
    {
        completeTimeDifference = _value;
    }

    public float GetCompleteTimeDifference()
    {
        return completeTimeDifference;
    }
}

[System.Serializable]
public class Alarm
{
    public bool hasAlarm = false;
    public int hour = 0;
    public int minute = 0;
    public Noon noon = Noon.AM;
}

public enum Noon
{
    AM, PM
}
