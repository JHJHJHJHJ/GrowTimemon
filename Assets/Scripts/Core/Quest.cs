using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour
{
    [Header("Quest Info")]
    public string title = null;
    public Sprite iconSprite = null;
    public List<SubQuest> subQuestList = new List<SubQuest>();
    public float rewardGoldAmount = 0;
    public float rewardDiaAmount = 0;
    bool isTimeLimit = false;
    public Alarm alarm;


    [Header("Components")]
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] Image icon = null;
    [SerializeField] GameObject timeLimit = null;
    [SerializeField] TextMeshProUGUI timeText = null;

    [HideInInspector] public bool hasClicked = false;

    private void Start()
    {
        UpdateQuestObject();
    }

    private void Update()
    {
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

        UpdateQuestObject();
    }

    void UpdateQuestObject()
    {
        titleText.text = title;
        icon.sprite = iconSprite;

        if(alarm != null && alarm.hasAlarm)
        {
            timeLimit.SetActive(true);
            timeText.text = alarm.hour + ":" + alarm.minute.ToString("D2") + " " + alarm.noon;
        }
        else
        {
            timeLimit.SetActive(false);
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
