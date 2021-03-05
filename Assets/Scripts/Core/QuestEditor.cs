using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEditor : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] int checkerGoldConstant = 20;
    [SerializeField] int timerGoldConstant = 20;
    [SerializeField] float minuteGoldWeight = 1f;

    [Header("References")]
    [SerializeField] QuestWindow questWindow = null;

    int[] currentRewards = new int[2]; // 0: gold 1: dia

    [HideInInspector] public Quest currentQuest = null;
    int currentPlateIndex;
    public Alarm alarmToEdit;

    private void Update()
    {
        UpdateRewards();
        ObservePlatesToOpenDelete();
    }

    public void Initialize()
    {
        currentQuest = null;
        currentRewards = new int[2] { 0, 0 };
        alarmToEdit = new Alarm();
        alarmToEdit.hasAlarm = false;
    }

    public void AddNewSubquestPlate() // 버튼에서 실행됨
    {
        FindObjectOfType<QuestWindow>().InstantiateSubquestPlate();
    }

    public void ToggleEditMode()
    {
        if (!questWindow.isEditing)
        {
            questWindow.isEditing = true;

            questWindow.SwitchEditor(questWindow.isEditing);
            questWindow.UpdateQuestInfoToEdit(currentQuest);

            alarmToEdit = currentQuest.alarm;
        }
        else
        {
            questWindow.isEditing = false;

            questWindow.OpenDetailWindow(currentQuest);
            alarmToEdit = null;
        }
    }

    public void ToggleIsRoutine() // 버튼에서 실행됨
    {
        if(!questWindow.isEditing) return;

        questWindow.SwitchIsRoutine(!questWindow.isRoutine);
    }

    public void PasteEditedQuest(Quest _questToEdit, Sprite _iconSprite)
    {
        List<SubQuest> newSubquestList = new List<SubQuest>();
        foreach (SubquestPlate plate in questWindow.plateList)
        {
            newSubquestList.Add(plate.GetSubquestInfo());
        }

        _questToEdit.SetupQuest(questWindow.GetInputedTitle(), _iconSprite, newSubquestList, currentRewards, alarmToEdit, questWindow.isRoutine);
    }

    void UpdateRewards()
    {
        if (!questWindow.isEditing) return;

        currentRewards[0] = 0;

        foreach (SubquestPlate plate in questWindow.plateList)
        {
            int amountToAdd = 0;

            if (plate.isTimer)
            {
                float time = 0f;
                float.TryParse(plate.inputField_time.text, out time);

                amountToAdd = (int)(time * minuteGoldWeight) + timerGoldConstant;
            }
            else
            {
                amountToAdd = checkerGoldConstant;
            }

            currentRewards[0] += amountToAdd;
        }

        questWindow.UpdateRewardsUI(currentRewards[0], alarmToEdit.hasAlarm);
    }

    void ObservePlatesToOpenDelete()
    {
        if (!questWindow.isEditing) return;

        foreach (SubquestPlate plate in questWindow.plateList)
        {
            if (plate.isWaiting)
            {
                currentPlateIndex = plate.index;
                questWindow.OpenSubquestDeletePopUp();

                plate.isWaiting = false;
            }
        }
    }

    public void DeleteSubquest()
    {
        Destroy(questWindow.plateList[currentPlateIndex].gameObject);
        questWindow.plateList.RemoveAt(currentPlateIndex);
        questWindow.UpdatePlatesIndex();

        questWindow.CloseSubquestDeletePopUp();
    }

    public void SetAlarm() // 버튼에서 실행됨
    {
        Alarm newAlarm = FindObjectOfType<AlarmSetWindow>().GetAlarmSetting();
        if(!newAlarm.hasAlarm)
        {
            newAlarm.hour = 0;
            newAlarm.minute = 0;
        }

        alarmToEdit = newAlarm;
        questWindow.CloseAlarmSetWindow();
        questWindow.UpdateAlarmUI(alarmToEdit);
    }

    public void OpenAlarmSet()
    {
        questWindow.OpenAlarmSetWindow(alarmToEdit);
    }
}
