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

    public void UseGold(int _amount)
    {
        gold -= _amount;
    }

    public void UseDia(int _amount)
    {
        dia -= _amount;
    }

    public bool CanBuy(string _type, int _price)
    {
        bool canBuy = false;
        if(_type == "gold") canBuy = (_price <= gold);
        else if(_type == "dia") canBuy = (_price <= dia);
        else
        {
            print("ERROR: CAN NOT RETURN CanBuy, " + _type);
        }

        return canBuy;
    }
}
