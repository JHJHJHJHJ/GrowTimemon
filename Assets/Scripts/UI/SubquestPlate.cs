﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI.ProceduralImage;
using UnityEngine.UI;

public class SubquestPlate : MonoBehaviour
{
    public bool isTimer = false;
    bool isEditing = false;

    [Header("Components")]
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] TextMeshProUGUI timeText = null;
    [SerializeField] ProceduralImage checkerBackground = null;
    [SerializeField] ProceduralImage timerBackground = null;
    [SerializeField] Image timerToggle = null;
    [SerializeField] TMP_InputField inputField_title = null;
    public TMP_InputField inputField_time = null;

    public void UpdatePlate(string _titleText, float _time)
    {
        if(_time != 0) isTimer = true;
        else isTimer = false;

        SwitchEditMode(false);
        UpdateTimer(isTimer);

        titleText.text = _titleText;
        if(isTimer) timeText.text = GetTimeTextToUpdate(_time);
    }

    void UpdateTimer(bool _isTimer)
    {
        if(_isTimer)
        {
            checkerBackground.gameObject.SetActive(false);
            timerBackground.gameObject.SetActive(true);

            timeText.gameObject.SetActive(true);

            timerToggle.gameObject.SetActive(true);
            timerToggle.color = Color.white;
        }
        else
        {
            checkerBackground.gameObject.SetActive(true);
            timerBackground.gameObject.SetActive(false);

            timeText.gameObject.SetActive(false);

            timerToggle.gameObject.SetActive(false);
        }
    }

    public void SwitchEditMode(bool _isEditing)
    {
        isEditing = _isEditing;

        inputField_title.gameObject.SetActive(_isEditing);
        inputField_time.gameObject.SetActive(_isEditing);

        titleText.gameObject.SetActive(!_isEditing);
        timeText.gameObject.SetActive(!_isEditing);

        inputField_time.gameObject.SetActive(isTimer);
        timeText.gameObject.SetActive(isTimer);        

        timerToggle.gameObject.SetActive(_isEditing);
    }

    string GetTimeTextToUpdate(float _time)
    {
        int time = (int)Math.Ceiling(_time);

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        string timeTextToUpdate = "";
        if (minutes > 0) timeTextToUpdate += minutes.ToString() + "분";
        if (seconds > 0) timeTextToUpdate += " " + seconds.ToString() + "초";

        return timeTextToUpdate;
    }

    //EDIT
    public void ToggleTimer()
    {
        if(!isEditing) return;

        isTimer = !isTimer;

        checkerBackground.gameObject.SetActive(!isTimer);
        timerBackground.gameObject.SetActive(isTimer);

        inputField_time.gameObject.SetActive(isTimer);

        if(isTimer) timerToggle.color = Color.white;
        else timerToggle.color = Color.gray;
    }

    public bool HasCompleted()
    {
        int falseCount = 0;

        if(inputField_title.text == "") falseCount++;

        if(isTimer)
        {
            if(inputField_time.text == "") falseCount++;

            float time = 0f;
            if(float.TryParse(inputField_time.text, out time))
            {
                if(time <= 0f) falseCount++;
            }
            else falseCount++;
        }

        if(falseCount <= 0) return true;
        else return false;
    }

    public SubQuest GetSubquestInfo()
    {
        SubQuest subQuest = new SubQuest();

        subQuest.isTimer = this.isTimer;
        subQuest.title = inputField_title.text;
        if(isTimer)
        {
            subQuest.second = float.Parse(inputField_time.text);
        }
        else
        {
            subQuest.second = 0f;
        }

        return subQuest;
    }
}
