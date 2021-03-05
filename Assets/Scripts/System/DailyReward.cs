using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DailyReward : MonoBehaviour
{
    [SerializeField] int diaToGet = 50;

    [SerializeField] GameObject dailyRewardBubble = null;
    [SerializeField] TextMeshProUGUI diaText = null;

    bool hasYetGot = false;

    private void Start() 
    {
        LoadHasYetGot();
        if(hasYetGot) 
        {
            ResetDailyReward();
        }
    }

    public void ResetDailyReward()
    {
        dailyRewardBubble.SetActive(true);
        diaText.text = diaToGet.ToString();

        hasYetGot = true;
    }

    public void GetDia()
    {
        FindObjectOfType<ResourceManager>().TakeReward(0, diaToGet);      
        
        dailyRewardBubble.SetActive(false);
        hasYetGot = false;
    }

    public void SaveHasYetGot()
    {
        ES3.Save<bool>("hasYetGot_DailyReward", hasYetGot);
    }

    void LoadHasYetGot()
    {
        if(ES3.KeyExists("hasYetGot_DailyReward"))
        {
            hasYetGot = ES3.Load<bool>("hasYetGot_DailyReward");
        }
    }
}
