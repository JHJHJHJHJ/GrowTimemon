using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using TMPro;

public class CharacterItem : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] int index = 0;

    [Header("Components")]
    [SerializeField] Character character = null;
    [SerializeField] Image lockImage = null;
    [SerializeField] ProceduralImage chooseButton = null;
    [SerializeField] TextMeshProUGUI chooseButtonText = null;
    
    CharacterManager characterManager;

    private void Awake() 
    {
        characterManager = FindObjectOfType<CharacterManager>();    
    }

    private void Start() 
    {
        UpdateItem();
    }

    public void UpdateItem()
    {
        bool isHaving = characterManager.GetCharacterIsHaving(index);
        bool isUsing = ( characterManager.currentIndex == index );

        GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.MidLight);

        if(isHaving)
        {
            character.gameObject.SetActive(true);
            lockImage.gameObject.SetActive(false);

            chooseButtonText.text = character.GetName();
            character.AnimateWait(!isUsing);

            if(isUsing)
            {
                GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.Mid);
                chooseButton.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.Dark);
                chooseButtonText.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.White);
            }
            else
            {
                chooseButton.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.White);
                chooseButtonText.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.Black);
            }
        }
        else
        {
            character.gameObject.SetActive(false);
            lockImage.gameObject.SetActive(true);

            chooseButtonText.text = "???";

            chooseButton.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.Light);
            chooseButtonText.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.Mid);
        }
    }
}
