using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EasyMobile;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public void RefreshNotifications(List<Quest> questList)
    {
        Notifications.CancelAllPendingLocalNotifications();

        foreach(Quest quest in questList)
        {
            if(quest.alarm.hasAlarm)
            {
                ScheduleLocalNotification(quest);
            }
        }
    }

    void ScheduleLocalNotification(Quest _quest)
    {
        NotificationContent content = PrepareNotificationContent(_quest);

        int hour = _quest.alarm.hour;
        if (hour == 12)
        {
            if (_quest.alarm.noon == Noon.AM) hour = 0;
        }
        else if (_quest.alarm.noon == Noon.PM)
        {
            hour += 12;
        }

        DateTime dateTimeAlarm = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 
            hour, _quest.alarm.minute, 0);

        DateTime dateTimeTarget;
        if(dateTimeAlarm <= DateTime.Now) dateTimeTarget = dateTimeAlarm.AddDays(1);
        else dateTimeTarget = dateTimeAlarm;

        var diffInSeconds = (dateTimeTarget - DateTime.Now).TotalSeconds;
        TimeSpan delay = TimeSpan.FromSeconds(diffInSeconds);

        Notifications.ScheduleLocalNotification(delay, content, NotificationRepeat.EveryDay);

        GetPendingLocalNotifications();
    }

    NotificationContent PrepareNotificationContent(Quest _quest)
    {
        NotificationContent content = new NotificationContent();

        content.title = _quest.title;
        content.subtitle = "준비되셨나요?";
        content.body = "퀘스트를 시작할 시간이에요!";

        return content;
    }

    void GetPendingLocalNotifications()
    {
        Notifications.GetPendingLocalNotifications(GetPendingLocalNotificationsCallback);
    }

    void GetPendingLocalNotificationsCallback(NotificationRequest[] pendingRequests)
    {
        foreach (var request in pendingRequests)
        {
            NotificationContent content = request.content;        

            print("Notification request ID: " + request.id);  
            print("Notification title: " + content.title);
            print("Notification body: " + content.body);
        }
    }
}
