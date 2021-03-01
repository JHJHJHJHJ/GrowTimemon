using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] int price = 50;
    [SerializeField] float characterPercent = 10f;
    [SerializeField] int[] goldsToGet; 

    [Header("References")]
    [SerializeField] GachaWindow gachaWindow = null;

    delegate void GetReward_D();
    GetReward_D getReward_D;  

    int charcterIndexToGet;
    int goldToGet;

    CharacterManager characterManager;
    UserData userData;

    private void Awake() 
    {
        characterManager = FindObjectOfType<CharacterManager>();    
        userData = FindObjectOfType<UserData>();
    }

    private void Update() 
    {
        gachaWindow.UpdateDiaText(CanBuy(), price);    
    }

    public void OpenGachaWindow()
    {
        gachaWindow.gameObject.SetActive(true);
        gachaWindow.Initiailize();
    }

    public void CloseGachaWindow()
    {
        gachaWindow.gameObject.SetActive(false);
    }

    public void StartGacha()
    {
        if(!CanBuy()) return;

        userData.UseDia(price);
        StartCoroutine(HandleGacha());
    }

    IEnumerator HandleGacha()
    {
        charcterIndexToGet = 0;
        goldToGet = 0;

        yield return StartCoroutine(gachaWindow.AnimateStartGacha());

        float chance = Random.Range(0f, 100f);
        if(characterManager.GetNotHaveCharcterIndexes().Count > 0 && chance <= characterPercent)
        {
            HandleCharcter();
        }
        else
        {
            HandleGold();
        }    
    }

    void HandleCharcter()
    {
        List<int> indexesCanGet = characterManager.GetNotHaveCharcterIndexes();

        charcterIndexToGet = indexesCanGet[Random.Range(0, indexesCanGet.Count)];
        Character characterToGet = characterManager.GetCharacter(charcterIndexToGet);

        gachaWindow.ShowCharacterResult(characterToGet);

        getReward_D = new GetReward_D(GetCharacter);
    }

    void HandleGold()
    {
        goldToGet = goldsToGet[Random.Range(0, goldsToGet.Length)];

        gachaWindow.ShowGoldResult(goldToGet);

        getReward_D = new GetReward_D(GetGold);
    }
    public void GetReward()
    {
        getReward_D();
        CloseGachaWindow();
    }

    void GetCharacter()
    {
        characterManager.CollectNewCharacter(charcterIndexToGet);
    }

    void GetGold()
    {
        userData.AddGold(goldToGet);
    }

    bool CanBuy()
    {
        return ( userData.GetDia() >= price );
    }
}
