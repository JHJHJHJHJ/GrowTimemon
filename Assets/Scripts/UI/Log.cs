using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Log : MonoBehaviour
{
    [Header("Componenets")]
    [SerializeField] Image circle = null;
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] TextMeshProUGUI timeText = null;
    [SerializeField] TextMeshProUGUI differenceText = null;

    [Header("Sprites")]
    [SerializeField] Sprite[] circleSprites = new Sprite[2];
 
    public void UpdateLog(SubQuest _subquest)
    {
        if(!_subquest.isTimer) // CHECKER
        {
            circle.sprite = circleSprites[0];
            titleText.text = _subquest.title;
            timeText.gameObject.SetActive(false);
            differenceText.gameObject.SetActive(false);
        }
        else // TIMER
        {
            circle.sprite = circleSprites[1];
            titleText.text = _subquest.title;

            timeText.gameObject.SetActive(true);
            timeText.text = ConvertTimeText(_subquest.second);

            differenceText.gameObject.SetActive(true);
            float difference = _subquest.GetCompleteTimeDifference();
            if(difference <= 0)
            {
                differenceText.color = Color.green;
                differenceText.text = "- " + ConvertTimeText(-1 * _subquest.GetCompleteTimeDifference());
            }
            else if(difference > 0)
            {
                differenceText.color = Color.red;
                differenceText.text = "+ " + ConvertTimeText(_subquest.GetCompleteTimeDifference());
            }
        }
    }

    string ConvertTimeText(float _seconds)
    {
        int secondsCeiled = (int)Math.Ceiling(_seconds);

        float hours = Mathf.FloorToInt(secondsCeiled / 3600);
        float timeWithoutHours = Mathf.FloorToInt(secondsCeiled % 3600);
        float minutes = Mathf.FloorToInt(secondsCeiled / 60);
        float seconds = Mathf.FloorToInt(secondsCeiled % 60);

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
}
