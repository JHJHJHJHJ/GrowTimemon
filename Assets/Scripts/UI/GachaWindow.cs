using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GachaWindow : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject closeButton = null;

    [Header("Idle")]
    [SerializeField] GameObject idle = null;
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] Animator egg = null;
    [SerializeField] Button gachaButton = null;
    [SerializeField] Image diaImage = null;
    [SerializeField] TextMeshProUGUI diaText = null;

    [Header("Result")]
    [SerializeField] GameObject result = null;
    [SerializeField] Button getButton = null;
    [SerializeField] TextMeshProUGUI text= null;
    [SerializeField] TextMeshProUGUI subText = null;
    [SerializeField] GameObject character = null;
    [SerializeField] GameObject gold = null;

    ColorManager colorManager;

    private void Awake() 
    {
        colorManager = FindObjectOfType<ColorManager>();    
    }

    private void Update() 
    {

    }

    public void Initiailize()
    {
        idle.SetActive(true);
        result.SetActive(false);

        titleText.gameObject.SetActive(true);
        gachaButton.gameObject.SetActive(true);
        closeButton.SetActive(true);

        colorManager.ChangeColors();
    }

    public IEnumerator AnimateStartGacha()
    {
        egg.SetTrigger("Gacha");
        titleText.gameObject.SetActive(false);
        gachaButton.gameObject.SetActive(false);
        closeButton.SetActive(false);

        yield return new WaitForSeconds(1.5f);
    }

    public void ShowCharacterResult(Character _characterToGet)
    {
        idle.SetActive(false);
        result.SetActive(true);

        gold.SetActive(false);
        character.SetActive(true);
        foreach(Transform child in character.transform)
        {   
            if(child.GetComponent<Character>()) Destroy(child.gameObject);
        }
        Character newCharacter = Instantiate(_characterToGet, character.transform.position, Quaternion.identity, character.transform);
        newCharacter.AnimateComplete();

        text.text = _characterToGet.GetName() + "가 나왔다!";

        colorManager.ChangeColors();
    }

    public void ShowGoldResult(int _goldToGet)
    {
        idle.SetActive(false);
        result.SetActive(true);

        character.SetActive(false);
        gold.SetActive(true);

        text.text = "꽝";
        subText.text = "대신 " +  _goldToGet + "골드를 드려요";

        colorManager.ChangeColors();
    }

    public void UpdateDiaText(bool _canBuy, int _price)
    {
        diaText.text = _price.ToString();

        if(_canBuy)
        {
            diaImage.color = Color.black;
            diaText.color = Color.black;
        }
        else
        {
            diaImage.color = Color.red;
            diaText.color = Color.red;
        }
    }
}
