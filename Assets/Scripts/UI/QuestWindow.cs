using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using System;

public class QuestWindow : MonoBehaviour
{
    [Header("Status")]
    public bool isCreating = false;
    public bool isEditing = false;
    public List<SubquestPlate> plateList = new List<SubquestPlate>();

    [Header("Info")]
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] ProceduralImage StartTime = null;
    [SerializeField] TextMeshProUGUI timeText = null;
    [SerializeField] TextMeshProUGUI goldAmountText = null;
    [SerializeField] GameObject dia = null;
    [SerializeField] Image diaImage = null;
    [SerializeField] TextMeshProUGUI diaAmountText = null;
    [SerializeField] GameObject noMoreReward = null;

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
    [SerializeField] GameObject routineButton = null;
    [SerializeField] AlarmSetWindow alarmSetWindow = null;

    Quest currentQuest;
    public bool isRoutine = false;


    bool inSwitch = true;
    bool outSwitch = true;

    private void Update()
    {
        UpdateConfirmButton();
        UpdateTimeLimitUI();
    }

    public void OpenDetailWindow(Quest _quest)
    {
        isCreating = false;
        isEditing = false;

        currentQuest = _quest;
        isRoutine = _quest.isRoutine;

        foreach (SubquestPlate subquestPlate in FindObjectsOfType<SubquestPlate>())
        {
            Destroy(subquestPlate.gameObject);
        }

        SwitchEditor(false);
        editButton.gameObject.SetActive(true);
        UpdateHasCleardReward(_quest.GetHasCleard());

        UpdateQuestInfo(_quest);
        UpdateAlarmUI(_quest.alarm);
        UpdateIsRoutine();

        InstantiateCurrentSubquestPlates(_quest);

        FindObjectOfType<ColorManager>().ChangeColors();
    }

    public void OpenCreateWindow()
    {
        isCreating = true;
        isEditing = true;

        isRoutine = false;

        plateList = new List<SubquestPlate>();

        foreach (SubquestPlate subquestPlate in FindObjectsOfType<SubquestPlate>())
        {
            Destroy(subquestPlate.gameObject);
        }

        confirmButton.gameObject.SetActive(true);

        SwitchEditor(true);
        editButton.gameObject.SetActive(false);
        UpdateAlarmUI(new Alarm());
        noMoreReward.SetActive(false);
        UpdateIsRoutine();

        goldAmountText.text = 0.ToString();

        dia.SetActive(false);
        diaAmountText.text = 0.ToString();

        FindObjectOfType<ColorManager>().ChangeColors();
    }

    public void SwitchEditor(bool _isEditing)
    {
        isEditing = _isEditing;

        startButton.gameObject.SetActive(!_isEditing);
        confirmButton.gameObject.SetActive(_isEditing);

        inputField_title.gameObject.SetActive(_isEditing);

        inputField_title.text = "";

        subquestAddButton.SetActive(_isEditing);

        if (_isEditing)
        {
            editButton.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.Dark);
            if (!isCreating) questDeleteButton.SetActive(true);
            else questDeleteButton.SetActive(false);

            diaImage.color = Color.black;
            diaAmountText.color = Color.black;
        }
        else
        {
            editButton.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.MidLight);
            questDeleteButton.SetActive(false);
        }

        StartTime.gameObject.SetActive(!isEditing);
        addAlarmButton.gameObject.SetActive(isEditing);
        UpdateIsRoutine();

        if(!isCreating) UpdateHasCleardReward(currentQuest.GetHasCleard());

        FindObjectOfType<ColorManager>().ChangeColors();
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
            if (isEditing) plate.UpdateEditInfo(_quest.subQuestList[plate.index]);
        }
    }

    void UpdateQuestInfo(Quest _quest)
    {
        titleText.text = _quest.title;

        UpdateRewardsUI((int)_quest.rewardGoldAmount, _quest.alarm.hasAlarm);
        UpdateTimeLimitUI();
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
        for (int i = 0; i < plateList.Count; i++)
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
            confirmButton.GetComponent<ColorChanger>().ChangeColorTo(ColorValue.Dark);
        }
        else confirmButton.GetComponent<ColorChanger>().ChangeColorTo(ColorValue.MidLight);
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

    public void UpdateIsRoutine()
    {
        if(isEditing) 
        {
            routineButton.SetActive(true);
            if(isCreating) SwitchIsRoutine(false);
            else SwitchIsRoutine(currentQuest.isRoutine);
           
        }
        else
        {
            if(currentQuest.isRoutine)
            {
                routineButton.SetActive(true);
                SwitchIsRoutine(true);
            }
            else
            {
                routineButton.SetActive(false);
            }
        }
    }

    public void SwitchIsRoutine(bool _isRoutine)
    {
        if(_isRoutine) 
        {
            isRoutine = true;
            routineButton.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.MidDark);
        }
        else 
        {
            isRoutine = false;
            routineButton.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.MidLight);
        }
    }

    public string GetInputedTitle()
    {
        return inputField_title.text;
    }

    public void UpdateRewardsUI(int _goldAmount, bool _hasAlarm)
    {
        if (_hasAlarm)
        {
            dia.SetActive(true);
            diaAmountText.text = 50.ToString();
        }
        else
        {
            dia.SetActive(false);
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
        if (_alarm != null && _alarm.hasAlarm)
        {
            timeText.text = _alarm.hour + ":" + _alarm.minute.ToString("D2") + " " + _alarm.noon;
            alarmSetText.text = _alarm.hour + ":" + _alarm.minute.ToString("D2") + " " + _alarm.noon;
        }
        else
        {
            alarmSetText.text = "+ 시작 시간";
        }

        if (isEditing)
        {
            StartTime.gameObject.SetActive(!isEditing);
            addAlarmButton.gameObject.SetActive(isEditing);
        }
        else
        {
            addAlarmButton.gameObject.SetActive(isEditing);
            if (_alarm != null && _alarm.hasAlarm)
            {
                StartTime.gameObject.SetActive(true);
            }
            else
            {
                StartTime.gameObject.SetActive(false);
            }
        }
    }

    void UpdateTimeLimitUI()
    {
        if (!currentQuest || isEditing || !currentQuest.alarm.hasAlarm) return;

        if (IsRightTime(currentQuest.alarm))
        {
            if (inSwitch)
            {
                diaImage.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.Dark);
                diaAmountText.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.Dark);

                StartTime.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.Mid);
                timeText.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.White);

                inSwitch = false;
                outSwitch = true;
            }

        }
        else
        {
            if (outSwitch)
            {
                diaImage.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.MidLight);
                diaAmountText.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.MidLight);

                StartTime.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.MidLight);
                timeText.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.White);

                inSwitch = true;
                outSwitch = false;
            }

        }
    }

    bool IsRightTime(Alarm _alarm)
    {
        int hour = _alarm.hour;
        if (hour == 12)
        {
            if (_alarm.noon == Noon.AM) hour = 0;
        }
        else if (_alarm.noon == Noon.PM)
        {
            hour += 12;
        }

        DateTime dateTime = new DateTime(
        DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
        hour, _alarm.minute, 0);

        DateTime before10m = dateTime.AddMinutes(-10d);
        DateTime after10m = dateTime.AddMinutes(10d);

        return (before10m <= DateTime.Now && DateTime.Now <= after10m);
    }

    void UpdateHasCleardReward(bool _hasCleared)
    {
        if(_hasCleared && !isEditing)
        {
            noMoreReward.SetActive(true);
        }
        else
        {
            noMoreReward.SetActive(false);
        }
    }
}