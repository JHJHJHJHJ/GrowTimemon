using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestWorkingView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI workingText = null;
    [SerializeField] TextMeshProUGUI questTitleText = null;
    [SerializeField] TextMeshProUGUI indexText = null;

    [Header("Checker")]
    [SerializeField] GameObject checker = null;
    [SerializeField] TextMeshProUGUI subquestTitleText = null; 
    

    [Header("Timer")]
    [SerializeField] GameObject timer = null;
    [SerializeField] TextMeshProUGUI timerSubquestTitleText = null; 

    public void OpenCheckerUI(string _questTitle, string _subquesttitle, string _index)
    {
        checker.SetActive(true);
        timer.SetActive(false);

        questTitleText.text = _questTitle;

        subquestTitleText.text = _subquesttitle;
        indexText.text = _index;
    }

    public void OpenTimerUI(string _questTitle, string _subquesttitle, string _index, float _second)
    {
        checker.SetActive(false);
        timer.SetActive(true);

        questTitleText.text = _questTitle;

        timerSubquestTitleText.text = _subquesttitle;
        indexText.text = _index;
        timer.GetComponent<Timer>().SetupTimer(_second);
    }

    public void UpdateWorkingText(bool _isOnTheQuest)
    {
        if(_isOnTheQuest) workingText.text = "퀘스트 수행 중...";
        else workingText.text = "퀘스트 완료!";
    }
}