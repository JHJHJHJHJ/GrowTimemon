using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Status")] public bool isRunning = false;

    [Header("Components")]
    [SerializeField] TextMeshProUGUI leftTimeText = null;
    [SerializeField] Slider timerSlider = null;
    [SerializeField] ProceduralImage sliderFill = null;
    [SerializeField] Button playButton = null;
    [SerializeField] Button completeButton = null;

    bool isTimeOver = false;
    float leftTime;
    float maxTime;

    private void Update()
    {
        UpdateTimer();
    }

    public void SetupTimer(float _second)
    {
        isRunning = false;
        isTimeOver = false;
        leftTime = _second;
        maxTime = _second;

        completeButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);

        leftTimeText.color = Color.black;
        sliderFill.GetComponent<ColorChanger>().ChangeColor();

        timerSlider.GetComponent<Animator>().SetBool("TimeOver", false);

        UpdateTimerUI();
    }

    public void StartTimer()
    {
        isRunning = true;
        playButton.gameObject.SetActive(false);
        completeButton.gameObject.SetActive(true);

        FindObjectOfType<Character>().AnimateWork(true);
    }

    void UpdateTimer()
    {
        if (!isRunning) return;

        leftTime = leftTime - Time.deltaTime;
        UpdateTimerUI();

        if(!isTimeOver)
        {
            if(leftTime <= 0f) TimeOver();
        }
    }

    public void PauseTimer()
    {
        isRunning = false;
        completeButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);

        FindObjectOfType<Character>().AnimateWork(false);
    }

    public void CompleteTimer()
    {
        //
    }

    void TimeOver()
    {
        isTimeOver = true;

        timerSlider.GetComponent<Animator>().SetBool("TimeOver", true);

        FindObjectOfType<HapticPlayer>().PlayTimeOverHaptic();
    }

    void UpdateTimerUI()
    {
        int leftTimeCeiled;
        string timeTextToUpdate = "";

        if(leftTime > -1f) leftTimeCeiled = (int)Math.Ceiling(leftTime);
        else 
        {
            leftTimeText.color = Color.yellow;
            leftTimeCeiled = -(int)Math.Ceiling(leftTime);

            timeTextToUpdate += "+ ";
        }

        float hours = Mathf.FloorToInt(leftTimeCeiled / 3600);
        float timeWithoutHours = Mathf.FloorToInt(leftTimeCeiled % 3600);
        float minutes = Mathf.FloorToInt(timeWithoutHours / 60);
        float seconds = Mathf.FloorToInt(timeWithoutHours % 60);

        if (hours > 0) timeTextToUpdate += hours.ToString() + "시간 ";
        if (minutes > 0) timeTextToUpdate += minutes.ToString() + "분 ";
        if (seconds >= 0)
        {
            if(seconds == 0 && (hours > 0 || minutes > 0)) {}
            else timeTextToUpdate += seconds + "초";
        } 

        leftTimeText.text = timeTextToUpdate;

        timerSlider.value = Mathf.Clamp((maxTime - leftTime) / maxTime, 0f, 1f);
    }

    public float GetLeftTime()
    {
        return leftTime;
    }
}
