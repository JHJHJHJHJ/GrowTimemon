using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    int index = 0;

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ChangeColors(index);
            index++;
        }
    }

    public void ChangeColors(int _index)
    {
        FindObjectOfType<ColorPalleteHolder>().ChangeCurrentPallete(_index);

        foreach(ColorChanger colorChanger in FindObjectsOfType<ColorChanger>())
        {
            colorChanger.ChangeColor();
        }
    }
}
