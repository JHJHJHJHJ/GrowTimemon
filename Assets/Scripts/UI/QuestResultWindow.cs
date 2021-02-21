using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class QuestResultWindow : MonoBehaviour
{
    [Header("Top")]
    [SerializeField] TextMeshProUGUI celebrationText = null;
    [SerializeField] Celebrations celebrations;
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] TextMeshProUGUI timeText = null;

    [Header("Clear Time")]
    [SerializeField] GameObject clearTime = null;
    [SerializeField] TextMeshProUGUI workedTimeText = null;
    [SerializeField] Slider clearTimeSlider = null;
    [SerializeField] TextMeshProUGUI clearTimeText = null;

    [Header("Additional Goals")]
    [SerializeField] GameObject additionalGoalsBackground = null;
    public AdditionalGoal[] additionalGoals = new AdditionalGoal[3];

    [Header("Rewards")]
    [SerializeField] GameObject gold = null;
    [SerializeField] GameObject dia = null;
    [SerializeField] TextMeshProUGUI goldText = null;
    [SerializeField] TextMeshProUGUI diaText = null;
    [SerializeField] GameObject getRewardButton = null;

    public void InitializeWindow()
    {
        celebrationText.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);

        clearTime.SetActive(false);
        additionalGoalsBackground.SetActive(false);
        foreach (AdditionalGoal additionalGoal in additionalGoals)
        {
            additionalGoal.SetActiveThis(false);
        }
        getRewardButton.SetActive(false);
        gold.gameObject.SetActive(false);
        dia.gameObject.SetActive(false);
    }


    public IEnumerator UpdateTop(Quest _quest)
    {
        UpdateCelebration();

        yield return new WaitForSeconds(0.5f);

        UpdateTitle(_quest);
    }

    void UpdateCelebration()
    {
        celebrationText.gameObject.SetActive(true);
        celebrationText.text = celebrations.GetRandomCelebration();
    }

    void UpdateTitle(Quest _quest)
    {
        titleText.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        titleText.text = _quest.title;
        // Update Time Text
    }

    public IEnumerator UpdateClearTime(Quest _Quest, float _totalDeltaTime, float _totalTime)
    {
        clearTime.SetActive(true);
        //Update workedTime Text

        float totalDeltaTimeToUpdate = 0f;
        UpdateSlider(totalDeltaTimeToUpdate, _totalTime);

        yield return new WaitForSeconds(0.5f);

        while (totalDeltaTimeToUpdate < _totalDeltaTime)
        {
            totalDeltaTimeToUpdate = Mathf.Clamp(
                totalDeltaTimeToUpdate + Time.deltaTime * 2f,
                0f, _totalDeltaTime); // easein-out 구현하기

            UpdateSlider(totalDeltaTimeToUpdate, _totalTime);

            yield return null;
        }
    }

    void UpdateSlider(float _totalDeltaTime, float _totalTime)
    {
        string clearTimeTextToUpdate = ConvertTimeText(_totalDeltaTime) + " / " + ConvertTimeText(_totalTime);
        clearTimeText.text = clearTimeTextToUpdate;

        float value = _totalDeltaTime / _totalTime;
        if (value <= 1f) clearTimeSlider.value = value;
        else clearTimeSlider.value = 1f;
    }

    public IEnumerator SetUpAdditionalGoals(Quest _Quest, float _totalDeltaTime, float _totalTime, float _accuratyStandard)
    {
        additionalGoalsBackground.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        // proper time
        additionalGoals[0].SetActiveThis(true);

        //if TRUE
        SetUpAdditionalGoalByAchieveing(0, true);

        yield return new WaitForSeconds(0.25f);

        // time attack
        additionalGoals[1].SetActiveThis(true);

        float totalDiffrences = _totalTime - _totalDeltaTime;
        if (totalDiffrences >= 0)
        {
            SetUpAdditionalGoalByAchieveing(1, true);
            additionalGoals[1].explainationText.text =
                "<color=#1BC544>" + ConvertTimeText(totalDiffrences) + "</color>" +
                " 빨리 완료했어요!";
        }
        else
        {
            SetUpAdditionalGoalByAchieveing(1, false);
            additionalGoals[1].explainationText.text =
                "<color=#FF1515>" + ConvertTimeText(-1 * totalDiffrences) + "</color>" +
                " 늦게 완료했어요." +
                "\n" +
                "조금 더 힘내서 집중해봐요!";
        }

        yield return new WaitForSeconds(0.25f);

        // accuracy
        additionalGoals[2].SetActiveThis(true);

        float accuracy = _totalDeltaTime / _totalTime;
        if (totalDiffrences >= 0)
        {
            if (accuracy >= _accuratyStandard)
            {
                SetUpAdditionalGoalByAchieveing(2, true);
                additionalGoals[2].explainationText.text =
                    "계획이 " + "<color=#1BC544>" + (int)(accuracy * 100f) + "%" + "</color>" + " 만큼 정확했어요!";
            }
            else
            {
                SetUpAdditionalGoalByAchieveing(2, false);
                additionalGoals[2].explainationText.text =
                    "계획이 " + "<color=#FF1515>" + (int)(accuracy * 100f) + "%" + "</color>" + " 만큼 정확했어요." +
                    "\n" +
                    "타이머 시간을 조절해 내 속도를 찾아봐요.";
            }
        }
        else
        {
            SetUpAdditionalGoalByAchieveing(2, false);
            additionalGoals[2].explainationText.text =
                "시간이 부족하기 때문일 수도 있어요." +
                "\n" +
                "타이머 시간을 조절해보세요.";
        }
    }
    public void UpdateRewards(int _gold, int _dia)
    {
        gold.gameObject.SetActive(true);
        if (_dia >= 0) dia.gameObject.SetActive(true);
        else dia.gameObject.SetActive(false);

        goldText.text = _gold.ToString();
        diaText.text = _dia.ToString();
    }

    public void Skip()
    {
        // 나중에 넣기
    }

    void SetUpAdditionalGoalByAchieveing(int _index, bool _isAchieved)
    {
        additionalGoals[_index].isAchieved = _isAchieved;

        additionalGoals[_index].SwitchCheckbox(_isAchieved);

        if (_isAchieved)
        {
            additionalGoals[_index].titleText.color = Color.black;
            additionalGoals[_index].titleText.fontStyle = FontStyles.Normal;
        }
        else
        {
            additionalGoals[_index].titleText.color = Color.gray;
            additionalGoals[_index].titleText.fontStyle = FontStyles.Strikethrough;
        }

        additionalGoals[_index].reward.SetActive(_isAchieved);
    }

    string ConvertTimeText(float _time)
    {
        float hours = Mathf.FloorToInt(_time / 3600);
        float timeWithoutHours = Mathf.FloorToInt(_time % 3600);
        float minutes = Mathf.FloorToInt(_time / 60);
        float seconds = Mathf.FloorToInt(_time % 60);

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

    public void ActiveRewardButton()
    {
        getRewardButton.SetActive(true);
    }
}

[System.Serializable]
public class AdditionalGoal
{
    public Image[] checkbox = new Image[2]; // 0: false - 1: true
    public TextMeshProUGUI titleText = null;
    public TextMeshProUGUI explainationText = null;
    public GameObject reward = null;
    public bool isAchieved = false;

    public void SwitchCheckbox(bool _isAchieved)
    {
        checkbox[0].gameObject.SetActive(!_isAchieved);
        checkbox[1].gameObject.SetActive(_isAchieved);
    }

    public void SetActiveThis(bool _isTrue)
    {
        checkbox[0].gameObject.SetActive(_isTrue);
        checkbox[1].gameObject.SetActive(_isTrue);

        titleText.gameObject.SetActive(_isTrue);
        explainationText.gameObject.SetActive(_isTrue);

        reward.gameObject.SetActive(_isTrue);
    }
}