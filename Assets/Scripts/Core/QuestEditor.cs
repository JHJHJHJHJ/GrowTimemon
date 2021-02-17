using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEditor : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] int checkerGoldWeight = 20;
    [SerializeField] int timerGoldConstant = 10;
    [SerializeField] float secondGoldWeight = 0.1f;

    [Header("References")]
    [SerializeField] QuestWindow questWindow = null;

    int[] currentRewards = new int[2]; // 0: gold 1: dia

    [HideInInspector] public Quest currentQuest = null;
    int currentPlateIndex;

    private void Update()
    {
        UpdateRewards();
        ObservePlatesToOpenDelete();
    }

    public void Initialize()
    {
        currentQuest = null;
        currentRewards = new int[2] { 0, 0 };
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
        }
        else
        {
            questWindow.isEditing = false;

            questWindow.OpenDetailWindow(currentQuest);
        }
    }

    public void PasteEditedQuest(Quest _questToEdit, Sprite _iconSprite)
    {
        List<SubQuest> newSubquestList = new List<SubQuest>();
        foreach (SubquestPlate plate in questWindow.plateList)
        {
            newSubquestList.Add(plate.GetSubquestInfo());
        }

        _questToEdit.SetupQuest(questWindow.GetInputedTitle(), _iconSprite, newSubquestList, currentRewards);
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

                amountToAdd = (int)(time * secondGoldWeight) + timerGoldConstant;
            }
            else
            {
                amountToAdd = checkerGoldWeight;
            }

            currentRewards[0] += amountToAdd;
        }

        questWindow.UpdateRewardsUI(currentRewards[0], currentRewards[1]);
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
}
