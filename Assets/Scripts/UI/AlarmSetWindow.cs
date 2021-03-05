using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AlarmSetWindow : MonoBehaviour
{
    [SerializeField] Button alarmButton = null;
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] GameObject bottom = null;
    [SerializeField] TMP_InputField hourInput = null;
    [SerializeField] TMP_InputField minuteInput = null;
    [SerializeField] TextMeshProUGUI amText = null;
    [SerializeField] TextMeshProUGUI pmText = null;
    [SerializeField] TextMeshProUGUI explanationText = null;

    Alarm alarmToSet;

    private void Update()
    {
        UpdateAlarmInfo();
        UpdateExplanationText();
    }

    public void Initialize(Alarm _alarm)
    {
        alarmToSet = new Alarm();
        if (_alarm == null || !_alarm.hasAlarm)
        {
            SwitchAlarm(false);

            if (DateTime.Now.Hour == 12)
            {
                alarmToSet.hour = DateTime.Now.Hour;
                SwitchNoon(Noon.PM);
            }
            else if (DateTime.Now.Hour > 12)
            {
                alarmToSet.hour = DateTime.Now.Hour - 12;
                SwitchNoon(Noon.PM);
            }
            else
            {
                alarmToSet.hour = DateTime.Now.Hour;
                SwitchNoon(Noon.AM);
            }
            alarmToSet.minute = DateTime.Now.Minute;
        }
        else
        {
            alarmToSet.hasAlarm = true;
            alarmToSet.hour = _alarm.hour;
            alarmToSet.minute = _alarm.minute;
            SwitchNoon(_alarm.noon);

            SwitchAlarm(true);
        }

        hourInput.text = alarmToSet.hour.ToString();
        minuteInput.text = alarmToSet.minute.ToString();

        UpdateExplanationText();

        FindObjectOfType<ColorManager>().ChangeColors();
    }

    public void ToggleAlarm()
    {
        alarmToSet.hasAlarm = !alarmToSet.hasAlarm;
        SwitchAlarm(alarmToSet.hasAlarm);

        FindObjectOfType<ColorManager>().ChangeColors();
    }

    void SwitchAlarm(bool _hasAlarm)
    {
        if (_hasAlarm)
        {
            alarmButton.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.Dark);
            titleText.text = "알림 시간을 설정하세요.";
            bottom.gameObject.SetActive(true);

            if(alarmToSet.noon == Noon.AM) SwitchToAM();
            else SwitchToPM();
        }
        else
        {
            alarmButton.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.MidLight);
            titleText.text = "알림을 설정하고 추가 보상을 받으세요.";
            bottom.gameObject.SetActive(false);
        }
    }

    public void SwitchNoon(Noon _noon)
    {
        if (_noon == Noon.AM)
        {
            SwitchToAM();
        }
        else if (_noon == Noon.PM)
        {
            SwitchToPM();
        }
    }

    public void SwitchToAM()
    {
        alarmToSet.noon = Noon.AM;

        if (pmText.IsActive())
        {
            pmText.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.MidLight);
            amText.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.Dark);
        }
    }

    public void SwitchToPM()
    {
        alarmToSet.noon = Noon.PM;

        if (pmText.IsActive())
        {
            pmText.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.Dark);
            amText.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.MidLight);
        }
    }

    public Alarm GetAlarmSetting()
    {
        return alarmToSet;
    }

    void UpdateExplanationText()
    {
        explanationText.text = "<b>" + GetTimeRange() + "</b>" + "\n" +
            "사이에 퀘스트를 시작하면" + "\n" + "50 다이아를 획득할 수 있습니다.";
    }

    string GetTimeRange()
    {
        int hour = alarmToSet.hour;
        if (hour == 12)
        {
            if (alarmToSet.noon == Noon.AM) hour = 0;
        }
        else if (alarmToSet.noon == Noon.PM)
        {
            hour += 12;
        }
        int minute = alarmToSet.minute;

        DateTime dateTime = new DateTime(
            DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
            hour, minute, 0);

        DateTime before10m = dateTime.AddMinutes(-10d);
        DateTime after10m = dateTime.AddMinutes(10d);

        return GetTimeString(before10m) + " ~ " + GetTimeString(after10m);
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

    public void UpdateAlarmInfo()
    {
        int hour = 0;
        if (int.TryParse(hourInput.text, out hour))
        {
            if (hour <= 0 || hour > 24)
            {
                hour = 12;
            }
            if (hour > 12 && hour <= 24)
            {
                hour -= 12;
                SwitchNoon(Noon.PM);
            }
        }
        else
        {
            hour = 12;
        }

        int minute = 0;
        if (int.TryParse(minuteInput.text, out minute))
        {
            if (minute < 0) minute = 0;
            if (minute >= 60) minute = 59;
        }
        else
        {
            minute = 0;
        }

        hourInput.text = hour.ToString();
        minuteInput.text = minute.ToString();

        alarmToSet.hour = hour;
        alarmToSet.minute = minute;
    }
}