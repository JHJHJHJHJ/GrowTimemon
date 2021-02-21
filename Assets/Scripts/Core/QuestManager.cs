using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] bool isOnTheQuest = false;

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

    private void Awake()
    {
        questEditor = GetComponent<QuestEditor>();
        questResultManager = GetComponent<QuestResultManager>();
        resourceManager = FindObjectOfType<ResourceManager>();
    }

    private void Start()
    {
        Quest[] quests = FindObjectsOfType<Quest>();
        for (int i = 0; i < quests.Length; i++)
        {
            questList.Add(quests[i]);
        }
    }

    private void Update()
    {
        ObserveTouchedQuest();
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
        FindObjectOfType<HapticPlayer>().PlayCompleteHaptic();

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

        questResultManager.OpenResultWindow(currentQuest);
    }

    public void CancelQuest()
    {
        isOnTheQuest = false;
        currentQuest = null;

        scrollView.SetActive(true);
        workingView.SetActive(false);

        FindObjectOfType<Character>().AnimateWork(false);
    }

    //EDIT

    public void ConfirmEdit()
    {
        if(!questWindow.CanConfirm()) return;

        if(questWindow.isCreating)
        {
            Quest newQuest = InstantiateNewQuestObject();
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

    public void DeleteCurrentQuest() // Yes 버튼에서 실행됨
    {
        questList.Remove(currentQuest);
        Destroy(currentQuest.gameObject);

        questWindow.CloseQuestDeletePopUp();
        questWindow.CloseWindow();
    }
}
