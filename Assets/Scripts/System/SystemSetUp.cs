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

    void OnApplicationQuit() 
    {
        SaveData();
    }   

    void SaveData()
    {
        questManager.SaveQuests();
        userData.SaveResources();
        colorPaletteHolder.SavePaletteIndex();
        encyclopedia.SavePaletteItemsStatus();
    } 
}