using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Config")]
    [TextArea] [SerializeField] string endedText = null;

    [Header("Components")]
    [SerializeField] TextMeshProUGUI leftTimeText = null;
    [SerializeField] Slider timerSlider = null;
    [SerializeField] Button playButton = null;
    [SerializeField] Button pauseButton = null;

    public bool isRunning = false;
    public bool hasEnded = false;
    float leftTime;
    float maxTime;

    private void Update()
    {
        UpdateTimer();
    }

    public void OpenTimer()
    {
        playButton.gameObject.SetActive(true);
    }

    public void StartTimer()
    {
        isRunning = true;
        playButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);

        FindObjectOfType<Character>().AnimateWork(true);
    }

    public void PauseTimer()
    {
        isRunning = false;
        pauseButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);

        FindObjectOfType<Character>().AnimateWork(false);
    }

    void UpdateTimer()
    {
        if (hasEnded || !isRunning) return;

        if (leftTime > 0)
        {
            leftTime = Mathf.Clamp(leftTime - Time.deltaTime, 0f, 999999f);
            UpdateTimerUI();
        }
        else
        {
            EndTimer();
        }
    }

    void EndTimer()
    {
        playButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);

        timerSlider.GetComponent<Animator>().SetBool("hasEnded", true);
        FindObjectOfType<Character>().AnimateWork(false);

        leftTimeText.text = endedText;

        isRunning = false;
        hasEnded = true;
    }

    public void SetupTimer(float _second)
    {
        isRunning = false;
        hasEnded = false;

        timerSlider.GetComponent<Animator>().SetBool("hasEnded", false);

        leftTime = _second;
        maxTime = _second;
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int leftTimeCeiled = (int)Math.Ceiling(leftTime);

        float minutes = Mathf.FloorToInt(leftTimeCeiled / 60);
        float seconds = Mathf.FloorToInt(leftTimeCeiled % 60);

        leftTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerSlider.value = (maxTime - leftTime) / maxTime;
    }
}
