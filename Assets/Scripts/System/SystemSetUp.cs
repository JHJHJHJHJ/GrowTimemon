using UnityEngine;

public class SystemSetUp : MonoBehaviour 
{
    QuestManager questManager;
    UserData userData;
    ColorPalleteHolder colorPaletteHolder;
    Encyclopedia encyclopedia;

    private void Awake() 
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; 

        questManager = FindObjectOfType<QuestManager>(); 
        userData = FindObjectOfType<UserData>();
        colorPaletteHolder = FindObjectOfType<ColorPalleteHolder>();
        encyclopedia = FindObjectOfType<Encyclopedia>();
    }

    private void OnApplicationQuit() 
    {
        SaveData();
    }   

    private void OnApplicationPause(bool pauseStatus) 
    {
        if(pauseStatus)
        {
            SaveData();
        }
    }

    void SaveData()
    {
        questManager.SaveQuests();
        userData.SaveResources();
        colorPaletteHolder.SavePaletteIndex();
        encyclopedia.SavePaletteItemsStatus();
    } 
}