using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EasyMobile;

public class QuestManager : MonoBehaviour
{
    [Header("Status")]
    public bool isOnTheQuest = false;

    [Header("Quest List")]
    [SerializeField] List<Quest> questList = new List<Quest>();

    [Header("Prefabs")]
    [SerializeField] GameObject questObject = null;

    [Header("References")]
    [SerializeField] Transform questParent = null;
    [SerializeField] GameObject scrollView = null;
    [SerializeField] GameObject workingView = null;
    [SerializeField] QuestWindow questWindow = null;

    [Header("Developing")]
    [SerializeField] string title = "default";
    [SerializeField] Sprite iconSprite = null;
    [SerializeField] List<SubQuest> subQuestList = new List<SubQuest>();

    Quest currentQuest = null;
    int currentSubquestIndex = 0;

    QuestEditor questEditor;
    QuestResultManager questResultManager;
    ResourceManager resourceManager;
    NotificationManager notificationManager;
    UserData userData;
    DateTime[] questTime = new DateTime[2];

    int nextID = 0;

    private void Awake()
    {
        questEditor = GetComponent<QuestEditor>();
        questResultManager = GetComponent<QuestResultManager>();
        resourceManager = FindObjectOfType<ResourceManager>();
        notificationManager = FindObjectOfType<NotificationManager>();
    }

    private void Start()
    {
        Initialize();

        notificationManager.RefreshNotifications(questList);
    }

    void Initialize()
    {
        LoadNextID();

        List<Quest> questList = LoadQuests();

        foreach(ES3InspectorInfo trash in FindObjectsOfType<ES3InspectorInfo>())
        {
            Destroy(trash.gameObject);
        }

        if(questList == null) return;

        foreach(Quest quest in questList)
        {
            Quest newQuest = InstantiateNewQuestObject();

            int[] rewards = new int[2] {(int)quest.rewardGoldAmount, (int)quest.rewardDiaAmount};
            newQuest.SetID(quest.id);
            newQuest.SetupQuest(quest.title, iconSprite, quest.subQuestList, rewards, quest.alarm);
        }
    }

    private void Update()
    {
        ObserveTouchedQuest();
    }

    public void SaveQuests()
    {
        ES3.Save<List<Quest>>("questList", questList);
    }

    List<Quest> LoadQuests()
    {
        if(ES3.KeyExists("questList")) return ES3.Load<List<Quest>>("questList");
        else return null;
    }

    void UpdateCurrentQuest(Quest _questToUpdate)
    {
        currentQuest = _questToUpdate;
        questEditor.currentQuest = _questToUpdate;
    }

    void ObserveTouchedQuest()
    {
        foreach (Quest quest in questList)
        {
            if (quest.hasClicked)
            {
                UpdateCurrentQuest(quest);
                OpenQuestDetialWindow(currentQuest);

                quest.hasClicked = false;
            }
        }
    }

    void OpenQuestDetialWindow(Quest _questToOpen)
    {
        questWindow.isCreating = false;
        questWindow.isEditing = false;

        questWindow.gameObject.SetActive(true);
        questWindow.OpenDetailWindow(_questToOpen);
    }

    public void OpenQuestCreateWindow() // 버튼에서 실행됨
    {
        UpdateCurrentQuest(null);
        questWindow.isCreating = true;
        questWindow.isEditing = true;
        questEditor.Initialize();

        questWindow.gameObject.SetActive(true);
        questWindow.OpenCreateWindow();
    }

    public void StartQuest() // 버튼에서 실행됨
    {
        questWindow.gameObject.SetActive(false);
        questResultManager.SetCurrentQuest(currentQuest);

        isOnTheQuest = true;

        FindObjectOfType<Character>().AnimateComplete();

        scrollView.SetActive(false);
        workingView.SetActive(true);

        FindObjectOfType<QuestWorkingView>().UpdateWorkingText(isOnTheQuest);

        currentSubquestIndex = 0;

        UpdateSubquest();

        questTime[0] = DateTime.Now;
    }

    public void TouchChecker()
    {
        if (isOnTheQuest)
        {
            if (!currentQuest.subQuestList[currentSubquestIndex].isTimer) // 체커
            {
                MoveToNextSubQuest();
            }
        }
        else // 완료되었을 때
        {
            questTime[1] = DateTime.Now;
            ShowResult();
        }
    }

    public void TouchTimer()
    {
        if (isOnTheQuest)
        {
            Timer timer = FindObjectOfType<Timer>();
            if (!timer.isRunning)
            {
                timer.StartTimer();
            }
            else
            {
                
            }
        }
    }

    public void CompleteTimer()
    {
        questResultManager.SetCurrentSubquestCompleteTimeDiffrence(currentSubquestIndex);

        MoveToNextSubQuest();
        FindObjectOfType<Character>().AnimateWork(false);
    }



    void MoveToNextSubQuest()
    {
        FindObjectOfType<Character>().AnimateComplete();

        if (currentSubquestIndex >= currentQuest.subQuestList.Count - 1) // 마지막일 때
        {
            QuestWorkingView questWorkingView = FindObjectOfType<QuestWorkingView>();

            isOnTheQuest = false;

            questWorkingView.OpenCheckerUI(currentQuest.title, "결과 확인", "");
            questWorkingView.UpdateWorkingText(isOnTheQuest);
        }
        else
        {
            currentSubquestIndex++;
            UpdateSubquest();
        }
    }

    private void UpdateSubquest()
    {
        List<SubQuest> currentSubQuestList = currentQuest.subQuestList;
        string indexText = (currentSubquestIndex + 1).ToString() + "/" + currentSubQuestList.Count;

        if (!currentSubQuestList[currentSubquestIndex].isTimer)
        {
            FindObjectOfType<QuestWorkingView>().OpenCheckerUI(currentQuest.title, currentSubQuestList[currentSubquestIndex].title, indexText);
        }
        else
        {
            FindObjectOfType<QuestWorkingView>().OpenTimerUI(
                currentQuest.title, currentSubQuestList[currentSubquestIndex].title, indexText,
                currentSubQuestList[currentSubquestIndex].second);
        }
    }

    void ShowResult()
    {
        scrollView.SetActive(true);
        workingView.SetActive(false);

        questResultManager.OpenResultWindow(currentQuest, questTime);
    }

    public void CancelQuest()
    {
        isOnTheQuest = false;
        currentQuest = null;

        scrollView.SetActive(true);
        workingView.SetActive(false);

        FindObjectOfType<Character>().AnimateWork(false);
    }

    public void ResetQuestsHasCleared()
    {
        foreach(Quest quest in questList)
        {
            quest.SetHasCleard(false);
        }
    }

    public void SaveQuestsHasCleard()
    {
        foreach(Quest quest in questList)
        {
            quest.SaveHasCleard();
        }
    }

    public void SaveNextID()
    {
        ES3.Save<int>("NextID", nextID);
    }

    void LoadNextID()
    {
        if(ES3.KeyExists("NextID"))
        {
            nextID = ES3.Load<int>("NextID");
        }
    }

    //EDIT

    public void ConfirmEdit()
    {
        if(!questWindow.CanConfirm()) return;

        if(questWindow.isCreating)
        {
            Quest newQuest = InstantiateNewQuestObject();
            newQuest.SetID(GetNextID());
            questEditor.PasteEditedQuest(newQuest, iconSprite);
            UpdateCurrentQuest(newQuest);
            questWindow.isCreating = false;
        }
        else
        {
            questEditor.PasteEditedQuest(currentQuest, currentQuest.iconSprite);
        }
        questWindow.isEditing = false;
        
        questWindow.CloseWindow();
        OpenQuestDetialWindow(currentQuest);

        notificationManager.RefreshNotifications(questList);
    }

    Quest InstantiateNewQuestObject()
    {
        FindObjectOfType<SwipeMenu>().DoNotScroll();

        GameObject createButton = GameObject.FindGameObjectWithTag("CreateButton");

        Quest newQuest = Instantiate(questObject, transform.position, Quaternion.identity, questParent).GetComponent<Quest>();
        questList.Add(newQuest);

        createButton.transform.SetAsLastSibling();
        createButton.transform.localScale = new Vector2(0.8f, 0.8f);

        FindObjectOfType<SwipeMenu>().DoNotScroll();

        return newQuest;
    }

    int GetNextID()
    {
        nextID++;
        return (nextID - 1);
    }

    public void DeleteCurrentQuest() // Yes 버튼에서 실행됨
    {
        ES3.DeleteKey("HasCleard_" + currentQuest.id);
        questList.Remove(currentQuest);
        Destroy(currentQuest.gameObject);

        questWindow.CloseQuestDeletePopUp();
        questWindow.CloseWindow();

        notificationManager.RefreshNotifications(questList);
    }
}
