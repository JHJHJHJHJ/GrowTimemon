using UnityEngine;
using UnityEngine.UI.ProceduralImage;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;


public class ColorChanger : MonoBehaviour 
{
    [SerializeField] ColorValue defaultColorValue;
    [SerializeField] ComponentType componentType;

    ColorPalleteHolder colorPalleteHolder;

    private void Awake() 
    {
        colorPalleteHolder = FindObjectOfType<ColorPalleteHolder>();    
        ChangeColor();
    }

    public void ChangeColor()
    {
        if(componentType == ComponentType.ProceduralImage) ChangePImageColor(defaultColorValue);
        else if (componentType == ComponentType.Image) ChangeImageColor(defaultColorValue);
        else if (componentType == ComponentType.TextMeshProUGUI) ChangeTextColor(defaultColorValue);
    }

    public void ChangeColorTo(ColorValue _colorValue)
    {
        if(componentType == ComponentType.ProceduralImage) ChangePImageColor(_colorValue);
        else if (componentType == ComponentType.Image) ChangeImageColor(_colorValue);
        else if (componentType == ComponentType.TextMeshProUGUI) ChangeTextColor(_colorValue);
    }

    void ChangePImageColor(ColorValue _colorvalue)
    {
        ProceduralImage pImage = GetComponent<ProceduralImage>();
        float alpha = pImage.color.a;
        Color colorToChange = colorPalleteHolder.GetColor(_colorvalue);
        pImage.color = new Color (colorToChange.r, colorToChange.g, colorToChange.b, alpha);
    }

    void ChangeImageColor(ColorValue _colorvalue)
    {
        Image image = GetComponent<Image>();
        float alpha = image.color.a;
        Color colorToChange = colorPalleteHolder.GetColor(_colorvalue);
        image.color = new Color (colorToChange.r, colorToChange.g, colorToChange.b, alpha);
    }

    void ChangeTextColor(ColorValue _colorvalue)
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        float alpha = text.color.a;
        Color colorToChange = colorPalleteHolder.GetColor(_colorvalue);
        text.color = new Color (colorToChange.r, colorToChange.g, colorToChange.b, alpha);
    }

    public void ChangeColorValueTo(ColorValue _colorValue)
    {
        defaultColorValue = _colorValue;
        ChangeColor();
    }
}

public enum ComponentType
{
    ProceduralImage,
    Image,
    TextMeshProUGUI
}