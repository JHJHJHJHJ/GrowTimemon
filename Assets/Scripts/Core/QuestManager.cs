using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
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

    bool isOnTheQuest = false;
    Quest currentQuest = null;
    int currentSubquestIndex = 0;

    QuestEditor questEditor;

    private void Awake()
    {
        questEditor = GetComponent<QuestEditor>();
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

    void ObserveTouchedQuest()
    {
        foreach (Quest quest in questList)
        {
            if (quest.hasClicked)
            {
                currentQuest = quest;
                OpenQuestDetialWindow(currentQuest);

                quest.hasClicked = false;
            }
        }
    }

    void OpenQuestDetialWindow(Quest _questToOpen)
    {
        questEditor.isCreating = false;

        questWindow.gameObject.SetActive(true);
        questWindow.OpenDetailWindow(_questToOpen);
    }

    public void OpenQuestCreateWindow() // 버튼에서 실행됨
    {
        currentQuest = null;
        questEditor.isCreating = true;

        questWindow.gameObject.SetActive(true);
        questWindow.OpenCreateWindow();
    }

    public void StartQuest() // 버튼에서 실행됨
    {
        questWindow.gameObject.SetActive(false);

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
            EndQuest();
        }
    }

    public void TouchTimer()
    {
        if (isOnTheQuest)
        {
            Timer timer = FindObjectOfType<Timer>();
            if (!timer.hasEnded)
            {
                if(timer.isRunning) timer.PauseTimer();
                else timer.StartTimer();
            }
            else
            {
                MoveToNextSubQuest();
            }
        }
    }

    void MoveToNextSubQuest()
    {
        if (currentSubquestIndex >= currentQuest.subQuestList.Count - 1) // 마지막일 때
        {
            QuestWorkingView questWorkingView = FindObjectOfType<QuestWorkingView>();

            isOnTheQuest = false;

            questWorkingView.OpenCheckerUI(currentQuest.title, "보상 받기", "");
            questWorkingView.UpdateWorkingText(isOnTheQuest);
        }
        else
        {
            FindObjectOfType<Character>().AnimateComplete();

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

    private void EndQuest()
    {
        scrollView.SetActive(true);
        workingView.SetActive(false);
    }

    //EDIT

    public void ConfirmEdit()
    {
        if(!questWindow.CanConfirm()) return;

        if(questEditor.isCreating)
        {
            Quest newQuest = InstantiateNewQuestObject();
            questEditor.EditQuest(newQuest, iconSprite);
            currentQuest = newQuest;
            questEditor.isCreating = false;
        }
        else
        {
            questEditor.EditQuest(currentQuest, currentQuest.iconSprite);
        }
        
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
}
