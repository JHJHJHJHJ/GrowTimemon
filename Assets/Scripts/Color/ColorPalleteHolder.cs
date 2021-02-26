using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalleteHolder : MonoBehaviour
{   
    [SerializeField] ColorPallete[] palletes;
    [SerializeField] ColorPallete currentColorPallete;
    int currentPaletteIndex = 0;

    void Awake()
    {

    }

    public void ChangeCurrentPallete(int _index)
    {
        currentPaletteIndex = _index;
        currentColorPallete = palletes[_index];
    }

    public Color GetColor(ColorValue _value)
    {
        Color colorToReturn;

        if(_value == ColorValue.Black) colorToReturn = Color.black;
        else if(_value == ColorValue.Gray) colorToReturn = Color.gray;
        else if(_value == ColorValue.White) colorToReturn = Color.white;
        else
        {
            colorToReturn = currentColorPallete.colors[(int)_value].color;
        }

        return colorToReturn;
    }

    public void SavePaletteIndex()
    {
        ES3.Save<int>("paletteIndex", currentPaletteIndex);
    }

    public int LoadCurrentPalette()
    {
        int index = 0;
        if(ES3.KeyExists("paletteIndex")) index = ES3.Load<int>("paletteIndex");

        ChangeCurrentPallete(index);
        return index;
    }
}
