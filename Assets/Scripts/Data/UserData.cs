using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    int gold;
    int dia;  

    public int GetGold()
    {
        return gold;
    }

    public int GetDia()
    {
        return dia;
    }

    public void AddGold(int _amount)
    {
        gold += _amount;
    }

    public void AddDia(int _amount)
    {
        dia += _amount;
    }
}
