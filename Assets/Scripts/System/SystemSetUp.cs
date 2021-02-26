using UnityEngine;

public class SystemSetUp : MonoBehaviour 
{
    QuestManager questManager;
    UserData userData;

    private void Awake() 
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; 

        questManager = FindObjectOfType<QuestManager>(); 
        userData = FindObjectOfType<UserData>();
    }

    void OnApplicationQuit() 
    {
        SaveData();
    }   

    void SaveData()
    {
        questManager.SaveQuests();
        userData.SaveResources();
    } 
}