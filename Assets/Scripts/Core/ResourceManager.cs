using UnityEngine;

public class ResourceManager : MonoBehaviour 
{
    UserData userData;
    ResourceDisplay resourceDisplay;

    private void Awake() 
    {
        userData = FindObjectOfType<UserData>();
        resourceDisplay = FindObjectOfType<ResourceDisplay>();    
    }

    public void TakeReward(int _goldAmount, int _diaAmount)
    {
        userData.AddGold(_goldAmount);
        userData.AddDia(_diaAmount);

        resourceDisplay.UpdateResourceText();
    }
}