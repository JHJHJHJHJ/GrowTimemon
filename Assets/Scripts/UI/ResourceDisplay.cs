using UnityEngine;
using TMPro;

public class ResourceDisplay : MonoBehaviour 
{
    [SerializeField] TextMeshProUGUI goldText = null;   
    [SerializeField] TextMeshProUGUI diaText = null;

    UserData userData;

    private void Awake() 
    {
        userData = FindObjectOfType<UserData>();    
    }

    private void Update() 
    {
        UpdateResourceText();
    }

    public void UpdateResourceText()
    {
        goldText.text = userData.GetGold().ToString();
        diaText.text = userData.GetDia().ToString();
    }
}