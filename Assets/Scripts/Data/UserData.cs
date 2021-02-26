using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;

public class UserData : MonoBehaviour
{
    [SerializeField] bool deleteSaveFile = false;
    int gold;
    int dia;  

    private void Awake() 
    {
        if(deleteSaveFile)  
        {
            ES3.DeleteFile("SaveFile.es3");
        }  
    }

    private void Start() 
    {
        LoadResources();    
    }

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

    // SAVE & LOAD

    public void SaveResources()
    {
        ES3.Save<int>("gold", gold);
        ES3.Save<int>("dia", dia);
    }

    void LoadResources()
    {
        if(ES3.KeyExists("gold")) gold = ES3.Load<int>("gold");
        if(ES3.KeyExists("dia")) dia = ES3.Load<int>("dia");
    }
}
