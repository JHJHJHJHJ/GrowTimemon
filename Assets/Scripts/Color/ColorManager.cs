using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    int index = 0;
    ColorPalleteHolder colorPaletteHolder;

    private void Awake() 
    {
        colorPaletteHolder = FindObjectOfType<ColorPalleteHolder>();
    }

    private void Start() 
    {
        ChangeColors();
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            index++;
            FindObjectOfType<ColorPalleteHolder>().ChangeCurrentPallete(index);
            ChangeColors();
        }
    }

    public void ChangeColors()
    {
        foreach(ColorChanger colorChanger in FindObjectsOfType<ColorChanger>())
        {
            colorChanger.ChangeColor();
        }
    }
}
