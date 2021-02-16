using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] GameObject timeLimit = null;
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
    [SerializeField] GameObject subquestAddButton = null;
    [SerializeField] Image confirmButton = null;

    bool isEditing = false;
    [HideInInspector] public List<SubquestPlate> plateList = new List<SubquestPlate>();

    private void Update()
    {
        UpdateConfirmButton();
    }

    public void OpenDetailWindow(Quest _quest)
    {
        isEditing = false;

        foreach (SubquestPlate subquestPlate in FindObjectsOfType<SubquestPlate>())
        {
            Destroy(subquestPlate.gameObject);
        }

        SwitchEditor(false);

        UpdateQuestInfo(_quest);
        InstantiateSubquestPlates(_quest);
    }

    public void OpenCreateWindow()
    {
        isEditing = true;

        plateList = new List<SubquestPlate>();

        foreach (SubquestPlate subquestPlate in FindObjectsOfType<SubquestPlate>())
        {
            Destroy(subquestPlate.gameObject);
        }

        confirmButton.gameObject.SetActive(true);
        confirmButton.color = Color.gray;

        SwitchEditor(true);

        goldAmountText.text = 0.ToString();
        dia.SetActive(false);
        diaAmountText.text = 0.ToString();
    }

    void SwitchEditor(bool _isEditor)
    {
        startButton.gameObject.SetActive(!_isEditor);
        confirmButton.gameObject.SetActive(_isEditor);

        inputField_title.gameObject.SetActive(_isEditor);

        inputField_title.text = "";

        subquestAddButton.SetActive(_isEditor);

        editButton.gameObject.SetActive(!_isEditor);
    }

    void UpdateQuestInfo(Quest _quest)
    {
        titleText.text = _quest.title;

        UpdateRewardsUI(_quest.rewardGoldAmount, _quest.rewardDiaAmount);
    }

    void InstantiateSubquestPlates(Quest _quest)
    {
        foreach (SubQuest subQuest in _quest.subQuestList)
        {
            SubquestPlate newPlate = Instantiate(subquestPlatePrefab, transform.position, Quaternion.identity, subquestsParent);

            newPlate.UpdatePlate(subQuest.title, subQuest.second);
        }

        subquestAddButton.transform.SetAsLastSibling();
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
}