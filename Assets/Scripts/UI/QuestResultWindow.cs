using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class QuestResultWindow : MonoBehaviour
{
    [Header("Top")]
    [SerializeField] TextMeshProUGUI celebrationText = null;
    [SerializeField] Celebrations celebrations;
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] GameObject startTime = null;
    [SerializeField] TextMeshProUGUI timeText = null;

    [Header("Clear Time")]
    [SerializeField] GameObject clearTime = null;
    [SerializeField] TextMeshProUGUI workedTimeText = null;
    [SerializeField] Slider clearTimeSlider = null;
    [SerializeField] TextMeshProUGUI clearTimeText = null;

    [Header("Additional Goals")]
    [SerializeField] GameObject additionalGoalsBackground = null;
    public AdditionalGoal[] additionalGoals = new AdditionalGoal[3];

    [Header("Rewards")]
    [SerializeField] GameObject gold = null;
    [SerializeField] GameObject dia = null;
    [SerializeField] TextMeshProUGUI goldText = null;
    [SerializeField] TextMeshProUGUI diaText = null;
    [SerializeField] GameObject getRewardButton = null;

    [Header("Log Window")]
    [SerializeField] Button logWindowButton = null;
    [SerializeField] GameObject logWindow = null;
    [SerializeField] Transform parent = null;
    [SerializeField] Log logPrefab = null;

    public void InitializeWindow()
    {
        celebrationText.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);

        clearTime.SetActive(false);
        additionalGoalsBackground.SetActive(false);
        foreach (AdditionalGoal additionalGoal in additionalGoals)
        {
            additionalGoal.SetActiveThis(false);
        }
        getRewardButton.SetActive(false);
        gold.gameObject.SetActive(false);
        dia.gameObject.SetActive(false);

        logWindowButton.gameObject.SetActive(false);
    }


    public void UpdateTop(Quest _quest)
    {
        UpdateCelebration();

        UpdateTitle(_quest);
    }

    void UpdateCelebration()
    {
        celebrationText.gameObject.SetActive(true);
        celebrationText.text = celebrations.GetRandomCelebration();
    }

    void UpdateTitle(Quest _quest)
    {
        titleText.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        titleText.text = _quest.title;

        if (_quest.alarm.hasAlarm)
        {
            startTime.SetActive(true);
            timeText.text = _quest.alarm.hour + ":" + _quest.alarm.minute.ToString("D2") + " " + _quest.alarm.noon;
        }
        else
        {
            startTime.SetActive(false);
        }
    }

    public void UpdateClearTime(Quest _Quest, float _totalDeltaTime, float _totalTime, DateTime[] _dateTimes)
    {
        int timerCount = 0;
        foreach(SubQuest subQuest in _Quest.subQuestList)
        {
            if(subQuest.isTimer) timerCount++;
        }

        if(timerCount <= 0) return;

        clearTime.SetActive(true);
        workedTimeText.text = GetTimeString(_dateTimes[0]) + " ~ " + GetTimeString(_dateTimes[1]);

        UpdateSlider(_totalDeltaTime, _totalTime);
    }

    void UpdateSlider(float _totalDeltaTime, float _totalTime)
    {
        string clearTimeTextToUpdate = ConvertTimeText(_totalDeltaTime) + " / " + ConvertTimeText(_totalTime);
        clearTimeText.text = clearTimeTextToUpdate;

        float value = _totalDeltaTime / _totalTime;
        if (value <= 1f) clearTimeSlider.value = value;
        else clearTimeSlider.value = 1f;
    }

    public void SetUpAdditionalGoals(Quest _Quest, float _totalDeltaTime, float _totalTime, float _accuratyStandard)
    {
        additionalGoalsBackground.SetActive(true);

        // proper time
        additionalGoals[0].SetActiveThis(true);

        if (_Quest.alarm.hasAlarm)
        {
            if (IsRightTime(_Quest.alarm))
            {
                SetUpAdditionalGoalByAchieveing(0, true);
                additionalGoals[0].explainationText.text = "제 시간에 시작했어요!";
            }
            else
            {
                SetUpAdditionalGoalByAchieveing(0, false);
                additionalGoals[0].explainationText.text = "다음엔 미루지 말아요!";
            }
        }
        else
        {
            SetUpAdditionalGoalByAchieveing(0, false);
            additionalGoals[0].explainationText.text = "시작 시간을 설정하면\n다이아를 받을 수 있어요.";
        }

        // 타이머 없는 순수 체크 퀘스트의 경우 아래 생략
        int timerCount = 0;
        foreach(SubQuest subQuest in _Quest.subQuestList)
        {
            if(subQuest.isTimer) timerCount++;
        }

        if(timerCount <= 0) 
        {
            SetUpAdditionalGoalByAchieveing(1, false);
            SetUpAdditionalGoalByAchieveing(2, false);
            
            return;
        }

        // time attack
        additionalGoals[1].SetActiveThis(true);

        float totalDiffrences = _totalTime - _totalDeltaTime;
        if (totalDiffrences >= 0)
        {
            SetUpAdditionalGoalByAchieveing(1, true);
            additionalGoals[1].explainationText.text =
                "<color=#1BC544>" + ConvertTimeText(totalDiffrences) + "</color>" +
                " 빨리 완료했어요!";
        }
        else
        {
            SetUpAdditionalGoalByAchieveing(1, false);
            additionalGoals[1].explainationText.text =
                "<color=#FF1515>" + ConvertTimeText(-1 * totalDiffrences) + "</color>" +
                " 늦게 완료했어요." +
                "\n" +
                "조금 더 힘내서 집중해봐요!";
        }

        // accuracy
        additionalGoals[2].SetActiveThis(true);

        float accuracy = _totalDeltaTime / _totalTime;
        if (totalDiffrences >= 0)
        {
            if (accuracy >= _accuratyStandard)
            {
                SetUpAdditionalGoalByAchieveing(2, true);
                additionalGoals[2].explainationText.text =
                    "계획이 " + "<color=#1BC544>" + (int)(accuracy * 100f) + "%" + "</color>" + " 만큼 정확했어요!";
            }
            else
            {
                SetUpAdditionalGoalByAchieveing(2, false);
                additionalGoals[2].explainationText.text =
                    "계획이 " + "<color=#FF1515>" + (int)(accuracy * 100f) + "%" + "</color>" + " 만큼 정확했어요." +
                    "\n" +
                    "타이머 시간을 조절해 내 속도를 찾아봐요.";
            }
        }
        else
        {
            SetUpAdditionalGoalByAchieveing(2, false);
            additionalGoals[2].explainationText.text =
                "시간이 부족하기 때문일 수도 있어요." +
                "\n" +
                "타이머 시간을 조절해보세요.";
        }
    }

    bool IsRightTime(Alarm _alarm)
    {
        int hour = _alarm.hour;
        if(hour == 12)
        {
            if(_alarm.noon == Noon.AM) hour = 0; 
        }
        else if(_alarm.noon == Noon.PM)
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
    public void UpdateRewards(int _gold, int _dia)
    {
        gold.gameObject.SetActive(true);
        if (_dia >= 0) dia.gameObject.SetActive(true);
        else dia.gameObject.SetActive(false);

        goldText.text = _gold.ToString();
        diaText.text = _dia.ToString();
    }

    public void Skip()
    {
        // 나중에 넣기
    }

    void SetUpAdditionalGoalByAchieveing(int _index, bool _isAchieved)
    {
        additionalGoals[_index].isAchieved = _isAchieved;

        additionalGoals[_index].SwitchCheckbox(_isAchieved);

        if (_isAchieved)
        {
            additionalGoals[_index].titleText.color = Color.black;
            additionalGoals[_index].titleText.fontStyle = FontStyles.Normal;
        }
        else
        {
            additionalGoals[_index].titleText.color = Color.gray;
            additionalGoals[_index].titleText.fontStyle = FontStyles.Strikethrough;
        }

        additionalGoals[_index].reward.SetActive(_isAchieved);
    }

    string ConvertTimeText(float _seconds)
    {
        float hours = Mathf.FloorToInt(_seconds / 3600);
        float timeWithoutHours = Mathf.FloorToInt(_seconds % 3600);
        float minutes = Mathf.FloorToInt(_seconds / 60);
        float seconds = Mathf.FloorToInt(_seconds % 60);

        string convertedTimeText = "";
        if (hours > 0) convertedTimeText += hours.ToString() + "시간 ";
        if (minutes > 0) convertedTimeText += minutes.ToString() + "분 ";
        if (seconds >= 0)
        {
            if (seconds == 0 && (hours > 0 || minutes > 0)) { }
            else convertedTimeText += seconds + "초";
        }

        return convertedTimeText;
    }

    string GetTimeString(DateTime _dateTime)
    {
        int h = _dateTime.Hour;
        int m = _dateTime.Minute;
        string timeString = "";

        if (_dateTime.Hour == 0)
        {
            timeString = "12" + ":" + m.ToString("D2") + " AM";
        }
        else if (_dateTime.Hour == 12)
        {
            timeString = h.ToString() + ":" + m.ToString("D2") + " PM";
        }
        else if (_dateTime.Hour > 12)
        {
            timeString = (h - 12).ToString() + ":" + m.ToString("D2") + " PM";
        }
        else
        {
            timeString = h.ToString() + ":" + m.ToString("D2") + " AM";
        }

        return timeString;
    }

    public void ActivateRewardButton()
    {
        logWindowButton.gameObject.SetActive(true);
        getRewardButton.SetActive(true);
    }

    public void OpenLogWindow(Quest _quest) 
    {
        logWindow.SetActive(true);

        foreach (Log log in FindObjectsOfType<Log>())
        {
            Destroy(log.gameObject);
        }

        foreach(SubQuest subQuest in _quest.subQuestList)
        {
            Log newLog = Instantiate(logPrefab, transform.position, Quaternion.identity, parent);
            newLog.UpdateLog(subQuest);
        }
    }

    public void CloseLogWindow() // 버튼에서 실행됨
    {
        logWindow.SetActive(false);
    }
}

[System.Serializable]
public class AdditionalGoal
{
    public Image[] checkbox = new Image[2]; // 0: false - 1: true
    public TextMeshProUGUI titleText = null;
    public TextMeshProUGUI explainationText = null;
    public GameObject reward = null;
    public bool isAchieved = false;

    public void SwitchCheckbox(bool _isAchieved)
    {
        checkbox[0].gameObject.SetActive(!_isAchieved);
        checkbox[1].gameObject.SetActive(_isAchieved);
    }

    public void SetActiveThis(bool _isTrue)
    {
        checkbox[0].gameObject.SetActive(_isTrue);
        checkbox[1].gameObject.SetActive(_isTrue);

        titleText.gameObject.SetActive(_isTrue);
        explainationText.gameObject.SetActive(_isTrue);

        reward.gameObject.SetActive(_isTrue);
    }
}