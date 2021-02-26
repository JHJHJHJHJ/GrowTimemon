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

    [Header("Palette")]
    [SerializeField] PaletteItem[] paletteItems = null;

    UserData userData;
    ColorManager ColorManager;

    private void Awake() 
    {
        userData = FindObjectOfType<UserData>();    
        ColorManager = FindObjectOfType<ColorManager>();
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

        ColorManager.ChangeColors(_index);
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
        encyclopediaWindow.SetActive(true);

        background.GetComponent<ColorChanger>().ChangeColor();
        characterButton.GetComponent<ColorChanger>().ChangeColor();
        paletteButton.GetComponent<ColorChanger>().ChangeColor();

        UpdatePaletteItems();
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
}

