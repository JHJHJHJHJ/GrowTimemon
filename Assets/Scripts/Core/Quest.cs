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
    public int rewardDiaAmount = 0;
    public int rewardGoldAmount = 0;
    bool isTimeLimit = false;


    [Header("Components")]
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] Image icon = null;

    [HideInInspector] public bool hasClicked = false;

    private void Start()
    {
        UpdateQuestObject();
    }

    private void Update() 
    {
        // string stringToPrint = null;

        // for(int i = 0; i < subQuestList.Count; i++)
        // {
        //     stringToPrint += i.ToString() + ": " + subQuestList[i].title + " / ";
        // }    

        // print(stringToPrint);
    }

    public void SetupQuest(string _title, Sprite _iconSprite, List<SubQuest> _subQuestList, int[] _rewardAmounts)
    {
        title = _title;
        iconSprite = _iconSprite;
        subQuestList = _subQuestList;

        rewardGoldAmount = _rewardAmounts[0];
        rewardDiaAmount = _rewardAmounts[1];

        UpdateQuestObject();
    }

    void UpdateQuestObject()
    {
        titleText.text = title;
        icon.sprite = iconSprite;
    }

    public void StartQuest()
    {
        hasClicked = true;
    }
}

[System.Serializable]
public class SubQuest
{
    public string title;
    public bool isTimer = false;
    public float second;
}
