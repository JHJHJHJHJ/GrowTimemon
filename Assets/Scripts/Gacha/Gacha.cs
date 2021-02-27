using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : MonoBehaviour
{
    [SerializeField] GameObject gachaWindow = null;

    public void OpenGachaWindow()
    {
        gachaWindow.SetActive(true);
    }

    public void CloseGachaWindow()
    {
        gachaWindow.SetActive(false);
    }
}
