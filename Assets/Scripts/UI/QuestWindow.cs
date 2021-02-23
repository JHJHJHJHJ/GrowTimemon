using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour
{
    [Header("Status")]
    public bool isCreating = false;
    public bool isEditing = false;
    public List<SubquestPlate> plateList = new List<SubquestPlate>();

    [Header("Info")]
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] GameObject StartTime = null;
    [SerializeField] TextMeshProUGUI timeText = null;
    [SerializeField] GameObject gold = null;
    [SerializeField] TextMeshProUGUI goldAmountText = null;
    [SerializeField] GameObject dia = null;
    [SerializeField] TextMeshProUGUI diaAmountText = null;

    [Header("Subquest")]
    [SerializeField] Transform subquestsParent = null;

    [Header("Prefabs")]
    [SerializeField] SubquestPlate subquestPlatePrefab = null;

    [Header("Detail")]
    [SerializeField] GameObject startButton = null;

    [Header("Editor")]
    [SerializeField] TMP_InputField inputField_title = null;
    [SerializeField] Image editButton = null;
    [SerializeField] Button addAlarmButton = null; 
    [SerializeField] TextMeshProUGUI alarmSetText = null;
    [SerializeField] GameObject subquestAddButton = null;
    [SerializeField] Image confirmButton = null;
    [SerializeField] GameObject subquestDeletePopUp = null;
    [SerializeField] GameObject questDeleteButton = null;
    [SerializeField] GameObject questDeletePopUp = null;
    [SerializeField] AlarmSetWindow alarmSetWindow = null;

    private void Update()
    {
        UpdateConfirmButton();
    }

    public void OpenDetailWindow(Quest _quest)
    {
        isCreating = false;
        isEditing = false;

        foreach (SubquestPlate subquestPlate in FindObjectsOfType<SubquestPlate>())
        {
            Destroy(subquestPlate.gameObject);
        }

        SwitchEditor(false);
        editButton.gameObject.SetActive(true);

        UpdateQuestInfo(_quest);
        UpdateAlarmUI(_quest.alarm);
        InstantiateCurrentSubquestPlates(_quest);
    }

    public void OpenCreateWindow()
    {
        isCreating = true;
        isEditing = true;

        plateList = new List<SubquestPlate>();

        foreach (SubquestPlate subquestPlate in FindObjectsOfType<SubquestPlate>())
        {
            Destroy(subquestPlate.gameObject);
        }

        confirmButton.gameObject.SetActive(true);
        confirmButton.color = Color.gray;

        SwitchEditor(true);
        editButton.gameObject.SetActive(false);
        UpdateAlarmUI(new Alarm());

        goldAmountText.text = 0.ToString();
        dia.SetActive(false);
        diaAmountText.text = 0.ToString();
    }

    public void SwitchEditor(bool _isEditing)
    {
        isEditing = _isEditing;

        startButton.gameObject.SetActive(!_isEditing);
        confirmButton.gameObject.SetActive(_isEditing);

        inputField_title.gameObject.SetActive(_isEditing);

        inputField_title.text = "";

        subquestAddButton.SetActive(_isEditing);

        if(_isEditing) 
        {
            editButton.color = new Color32(255, 255, 255, 255);
            if(!isCreating) questDeleteButton.SetActive(true);
            else questDeleteButton.SetActive(false);
        }
        else 
        {
            editButton.color = new Color32(255, 255, 255, 40);
            questDeleteButton.SetActive(false);
        }

        StartTime.SetActive(!isEditing);
        addAlarmButton.gameObject.SetActive(isEditing);
    }

    public void UpdateQuestInfoToEdit(Quest _quest)
    {
        inputField_title.text = _quest.title;

        SwitchPlatesEdieMode(_quest);
    }

    public void SwitchPlatesEdieMode(Quest _quest)
    {
        foreach (SubquestPlate plate in plateList)
        {
            plate.SwitchEditMode(isEditing);
            if(isEditing) plate.UpdateEditInfo(_quest.subQuestList[plate.index]);
        }
    }

    void UpdateQuestInfo(Quest _quest)
    {
        titleText.text = _quest.title;

        UpdateRewardsUI((int)_quest.rewardGoldAmount, (int)_quest.rewardDiaAmount);
    }

    void InstantiateCurrentSubquestPlates(Quest _quest)
    {
        plateList = new List<SubquestPlate>();

        foreach (SubQuest subQuest in _quest.subQuestList)
        {
            SubquestPlate newPlate = Instantiate(subquestPlatePrefab, transform.position, Quaternion.identity, subquestsParent);

            newPlate.UpdatePlate(subQuest.title, subQuest.second);
            plateList.Add(newPlate);
        }
        UpdatePlatesIndex();
        subquestAddButton.transform.SetAsLastSibling();
    }

    
    public void UpdatePlatesIndex()
    {
        for(int i = 0; i < plateList.Count; i++)
        {
            plateList[i].index = i;   
        }
    }

    public void CloseWindow() // 버튼에서 실행됨
    {
        isEditing = false;

        this.gameObject.SetActive(false);
    }

    //EDIT
    public void InstantiateSubquestPlate()
    {
        SubquestPlate newPlate = Instantiate(subquestPlatePrefab, transform.position, Quaternion.identity, subquestsParent);

        newPlate.SwitchEditMode(true);

        subquestAddButton.transform.SetAsLastSibling();

        plateList.Add(newPlate);

        UpdatePlatesIndex();
    }

    void UpdateConfirmButton()
    {
        if (!isEditing) return;

        if (CanConfirm())
        {
            confirmButton.color = Color.black;
        }
        else confirmButton.color = Color.gray;
    }

    public bool CanConfirm()
    {
        bool canConfirm = true;

        if (inputField_title.text == "") canConfirm = false;

        if (plateList.Count <= 0) canConfirm = false;
        else
        {
            foreach (SubquestPlate plate in plateList)
            {
                if (!plate.HasCompleted())
                {
                    canConfirm = false;
                    break;
                }
            }
        }

        return canConfirm;
    }

    public string GetInputedTitle()
    {
        return inputField_title.text;
    }

    public void UpdateRewardsUI(int _goldAmount, int _diaAmount)
    {
        if (_diaAmount <= 0) dia.SetActive(false);
        else
        {
            dia.SetActive(true);
            diaAmountText.text = _diaAmount.ToString();
        }
        
        goldAmountText.text = _goldAmount.ToString();
    }

    public void OpenSubquestDeletePopUp()
    {
        subquestDeletePopUp.SetActive(true);
    }

    public void CloseSubquestDeletePopUp()
    {
        subquestDeletePopUp.SetActive(false);
    }

    public void OpenQuestDeletePopUp()
    {
        questDeletePopUp.SetActive(true);
    }

    public void CloseQuestDeletePopUp()
    {
        questDeletePopUp.SetActive(false);
    }

    public void OpenAlarmSetWindow(Alarm _alarm)
    {
        alarmSetWindow.gameObject.SetActive(true);
        alarmSetWindow.Initialize(_alarm);
    }

    public void CloseAlarmSetWindow()
    {
        alarmSetWindow.gameObject.SetActive(false);
    }

    public void UpdateAlarmUI(Alarm _alarm)
    {
        if(_alarm != null && _alarm.hasAlarm)
        {
            timeText.text = _alarm.hour + ":" + _alarm.minute.ToString("D2") + " " + _alarm.noon;
            alarmSetText.text = _alarm.hour + ":" + _alarm.minute.ToString("D2") + " " + _alarm.noon;
        }
        else
        {
            alarmSetText.text = "+ 시작 시간";
        }

        if(isEditing)
        {
            StartTime.SetActive(!isEditing);
            addAlarmButton.gameObject.SetActive(isEditing);
        }
        else
        {
            addAlarmButton.gameObject.SetActive(isEditing);
            if(_alarm != null && _alarm.hasAlarm)
            {
                StartTime.SetActive(true);
            }
            else
            {
                StartTime.SetActive(false);
            }
        }
    }
}