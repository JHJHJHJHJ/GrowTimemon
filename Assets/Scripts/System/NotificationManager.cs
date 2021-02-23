using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EasyMobile;

public class NotificationManager : MonoBehaviour
{
    NotificationContent PrepareNotificationContent()
    {
        NotificationContent content = new NotificationContent();

        content.title = "Demo Notification";
        content.subtitle = "Demo Subtitle";
        content.body = "This is a demo notification.";

        return content;
    }

    void ScheduleLocalNotification()
    {
        NotificationContent content = PrepareNotificationContent();

        DateTime dateTimeNow = DateTime.Now;

        DateTime dateTimeTarget = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 15, 0 ,0);

        var diffInSeconds = (dateTimeTarget - dateTimeNow).TotalSeconds;
        TimeSpan delay = TimeSpan.FromSeconds(diffInSeconds);

        Notifications.ScheduleLocalNotification(delay, content, NotificationRepeat.EveryHour);

        GetPendingLocalNotifications();
    }

    void GetPendingLocalNotifications()
    {
        Notifications.GetPendingLocalNotifications(GetPendingLocalNotificationsCallback);
    }

    // Callback.
    void GetPendingLocalNotificationsCallback(NotificationRequest[] pendingRequests)
    {
        foreach (var request in pendingRequests)
        {
            NotificationContent content = request.content;        

            Debug.Log("Notification request ID: " + request.id);  
            Debug.Log("Notification title: " + content.title);
            Debug.Log("Notification body: " + content.body);
        }
    }
}
