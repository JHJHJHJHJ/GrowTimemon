using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI.ProceduralImage;

public class Encyclopedia : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject encyclopediaWindow = null;
    [SerializeField] ProceduralImage background = null;
    [SerializeField] ProceduralImage characterButton = null;
    [SerializeField] ProceduralImage paletteButton = null;

    [Header("Character")]
    [SerializeField] GameObject characterView = null;
    [SerializeField] CharacterItem[] characterItems = null;

    [Header("Palette")]
    [SerializeField] GameObject paletteView = null;
    [SerializeField] PaletteItem[] paletteItems = null;

    UserData userData;
    ColorManager colorManager;
    ColorPalleteHolder colorPalleteHolder;
    CharacterManager characterManager;

    private void Awake() 
    {
        userData = FindObjectOfType<UserData>();    
        colorManager = FindObjectOfType<ColorManager>();
        colorPalleteHolder = FindObjectOfType<ColorPalleteHolder>();
        characterManager = FindObjectOfType<CharacterManager>();
    }

    public void BuyPaletteItem(int _index)
    {
        PaletteItem itemToBuy = paletteItems[_index];

        if(!itemToBuy.CanBuyThis()) return;

        itemToBuy.Have();
        userData.UseGold(itemToBuy.GetPrice());
        ChangePalette(_index);
    }

    public void ChangePalette(int _index)
    {
        ChoosePaletteItem(_index);

        FindObjectOfType<ColorPalleteHolder>().ChangeCurrentPallete(_index);
        colorManager.ChangeColors();
    }

    void ChoosePaletteItem(int _index)
    {
        for (int i = 0; i < paletteItems.Length; i++)
        {
            bool isUsing = (i == _index);

            paletteItems[i].SwitchIsUsing(isUsing);
        }
        UpdatePaletteItems();
    }

    public void OpenWindow()
    {
        if(FindObjectOfType<QuestManager>().isOnTheQuest) return;

        encyclopediaWindow.SetActive(true);

        background.GetComponent<ColorChanger>().ChangeColor();
        characterButton.GetComponent<ColorChanger>().ChangeColor();
        paletteButton.GetComponent<ColorChanger>().ChangeColor();

        UpdatePaletteItems();
        UpdateCharacterItems();
    }

    void UpdatePaletteItems()
    {
       foreach(PaletteItem paletteItem in paletteItems)
       {
           paletteItem.UpdateItem();
       } 
    }

    public void CloseWindow()
    {
        encyclopediaWindow.SetActive(false);
    }

    public void SavePaletteItemsStatus()
    {
        foreach(PaletteItem paletteItem in paletteItems)
        {
            paletteItem.SaveStatus();
        }
    }

    public void OpenPaletteView()
    {
        if(paletteView.activeSelf) return;

        characterButton.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.MidLight);
        paletteButton.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.MidDark);

        characterView.SetActive(false);
        paletteView.SetActive(true);

        UpdatePaletteItems();
    }

    public void OpenCharacterView()
    {
        if(!paletteView.activeSelf) return;

        characterButton.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.MidDark);
        paletteButton.GetComponent<ColorChanger>().ChangeColorValueTo(ColorValue.MidLight);

        characterView.SetActive(true);
        paletteView.SetActive(false);

        UpdateCharacterItems();
        colorManager.ChangeColors();
    }   

    void UpdateCharacterItems()
    {
        foreach(CharacterItem characterItem in characterItems)
        {
            characterItem.UpdateItem();
        }
    }

    public void ChooseCharacter(int _index)
    {
        bool isHaving = characterManager.GetCharacterIsHaving(_index);
        bool isUsing = ( characterManager.currentIndex == _index );

        if(isHaving && !isUsing)
        {
            characterManager.ChooseThisCharacter(_index);
            UpdateCharacterItems();
        }
    }
}

