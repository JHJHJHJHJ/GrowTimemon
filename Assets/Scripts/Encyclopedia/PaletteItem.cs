using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class PaletteItem : MonoBehaviour
{
    [SerializeField] string paletteName = null;
    [SerializeField] bool isHaving = false;
    [SerializeField] bool isUsing = false;
    [SerializeField] int price = 1000;

    [Header("Components")]
    [SerializeField] GameObject buyButton = null;
    [SerializeField] Image priceIcon = null;
    [SerializeField] TextMeshProUGUI priceText = null;
    [SerializeField] GameObject changeButton = null;

    [Space]
    [SerializeField] ProceduralImage usingPanel = null;
    [SerializeField] ProceduralImage background = null;
    [SerializeField] ProceduralImage mid = null;
    [SerializeField] ProceduralImage midDark = null;

    [Header("Palette")]
    [SerializeField] ColorPallete palette = null;

    private void Start() 
    {
        LoadStatus();

        UpdateColors();
        UpdateItem();    
    }

    public void Have()
    {
        isHaving = true;
    }

    public void SwitchIsUsing(bool _isUsing)
    {
        isUsing = _isUsing;
    }

    public void UpdateColors()
    {
        background.color = palette.colors[(int)ColorValue.MidLight].color;
        mid.color = palette.colors[(int)ColorValue.Mid].color;
        midDark.color = palette.colors[(int)ColorValue.MidDark].color;
        usingPanel.color = palette.colors[(int)ColorValue.Dark].color;
    }

    public void UpdateItem()
    {
        SwitchButtons();
        UpdatePriceText();
    }

    public bool CanBuyThis()
    {
        return FindObjectOfType<UserData>().CanBuy("gold", price);
    }

    void UpdatePriceText()
    {
        if(!CanBuyThis()) 
        {
            priceIcon.color = Color.red;
            priceText.color = Color.red;
        }
        else 
        {
            priceIcon.color = Color.black;
            priceText.color = Color.black;
        }

        priceText.text = price.ToString();
    }

    void SwitchButtons()
    {
        if(!isHaving)
        {
            buyButton.SetActive(true);
            changeButton.SetActive(false);
            usingPanel.gameObject.SetActive(false);
        }
        else
        {
            buyButton.SetActive(false);

            if(isUsing)
            {
                changeButton.SetActive(false);
                usingPanel.gameObject.SetActive(true);
            }
            else
            {
                changeButton.SetActive(true);
                usingPanel.gameObject.SetActive(false);
            }
        }
    }

    public int GetPrice()
    {
        return price;
    }

    public void SaveStatus()
    {
        bool[] status = new bool[2] {isHaving, isUsing};
        ES3.Save<bool[]>("paletteItemStatus_" + paletteName, status);
    }

    void LoadStatus()
    {
        if(!ES3.KeyExists("paletteItemStatus_" + paletteName)) return;

        bool[] status = new bool[2];
        status = ES3.Load<bool[]>("paletteItemStatus_" + paletteName);

        isHaving = status[0];
        isUsing = status[1];
    }
}
