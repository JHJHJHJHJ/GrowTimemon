using UnityEngine;

public class SystemSetUp : MonoBehaviour
{
    QuestManager questManager;
    UserData userData;
    ColorPalleteHolder colorPaletteHolder;
    Encyclopedia encyclopedia;
    CharacterManager characterManager;
    DailyReward dailyReward;
    RealTimeHandler realTimeHandler;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        questManager = FindObjectOfType<QuestManager>();
        userData = FindObjectOfType<UserData>();
        colorPaletteHolder = FindObjectOfType<ColorPalleteHolder>();
        encyclopedia = FindObjectOfType<Encyclopedia>();
        characterManager = FindObjectOfType<CharacterManager>();
        dailyReward = FindObjectOfType<DailyReward>();
        realTimeHandler = FindObjectOfType<RealTimeHandler>();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveData();

            if (questManager.isOnTheQuest)
            {
                questManager.CancelQuest();
            }
        }
    }

    void SaveData()
    {
        questManager.SaveQuests();
        questManager.SaveQuestsHasCleard();
        questManager.SaveNextID();

        userData.SaveResources();

        colorPaletteHolder.SavePaletteIndex();

        encyclopedia.SavePaletteItemsStatus();

        characterManager.SaveCharacterData();

        dailyReward.SaveHasYetGot();

        realTimeHandler.SaveResetTime();
    }
}