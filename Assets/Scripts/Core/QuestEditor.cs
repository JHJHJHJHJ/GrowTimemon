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

    public bool isCreating = false;
    public bool isEditing = false;

    private void Update() 
    {
        UpdateRewards();
    }

    public void Initialize()
    {
        currentRewards = new int[2] {0, 0};
    }

    public void AddNewSubquestPlate()
    {
        FindObjectOfType<QuestWindow>().InstantiateSubquestPlate();
    }

    public void EditQuest(Quest _questToEdit, Sprite _iconSprite)
    {
        List<SubQuest> newSubquestList = new List<SubQuest>();
        foreach(SubquestPlate plate in questWindow.plateList)
        {
            newSubquestList.Add(plate.GetSubquestInfo());
        }

        _questToEdit.SetupQuest(questWindow.GetInputedTitle(), _iconSprite, newSubquestList, currentRewards);
    }

    void UpdateRewards()
    {
        if(!isEditing) return;

        currentRewards[0] = 0;

        foreach(SubquestPlate plate in questWindow.plateList)
        {
            int amountToAdd = 0;

            if(plate.isTimer)
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
}
