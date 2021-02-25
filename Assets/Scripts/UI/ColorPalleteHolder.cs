using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalleteHolder : MonoBehaviour
{   
    [SerializeField] ColorPallete[] palletes;
    [SerializeField] ColorPallete currentColorPallete;

    void Awake()
    {

    }

    public void ChangeCurrentPallete(int _index)
    {
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
}
