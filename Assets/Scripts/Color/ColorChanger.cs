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
    }

    private void Start() 
    {
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
        pImage.color = colorPalleteHolder.GetColor(_colorvalue);
    }

    void ChangeImageColor(ColorValue _colorvalue)
    {
        Image image = GetComponent<Image>();
        image.color = colorPalleteHolder.GetColor(_colorvalue);
    }

    void ChangeTextColor(ColorValue _colorvalue)
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.color = colorPalleteHolder.GetColor(_colorvalue);
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