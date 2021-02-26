using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ColorPallete", menuName = "GrowTimemon/ColorPallete", order = 0)]
public class ColorPallete : ScriptableObject 
{
    public string palleteName; 
    public ColorP[] colors;
}

[System.Serializable]
public class ColorP
{
    public ColorValue value;
    public Color color;    
}

public enum ColorValue
{
    Light = 0, 
    MidLight = 1, 
    Mid = 2, 
    MidDark = 3, 
    Dark = 4,

    White = 5,
    Gray = 6,
    Black = 7 
}
