using UnityEngine;
using System;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour 
{
    [SerializeField] CharacterCollectable[] characterCollectables = null;
    [SerializeField] Character currentCharacter = null;
    [SerializeField] Transform instantiateParent = null;

    public int currentIndex = 0;

    private void Start()
    {
        LoadCharacterData();
        InstantiateCharacter();
    }

    public void InstantiateCharacter()
    {
        Destroy(currentCharacter.gameObject);

        currentCharacter = Instantiate(characterCollectables[currentIndex].character,
            instantiateParent.position, Quaternion.identity, instantiateParent);
    }

    public void CollectNewCharacter(int _index)
    {
        characterCollectables[_index].isHaving = true;
    }

    public bool GetCharacterIsHaving(int _index)
    {
        return characterCollectables[_index].isHaving;
    }

    public List<int> GetNotHaveCharcterIndexes()
    {
        List<int> indexes = new List<int>();

        for (int i = 0; i < characterCollectables.Length; i++)
        {
            if(!characterCollectables[i].isHaving)
            {
                indexes.Add(i);
            }
        }

        return indexes;
    }

    public void ChooseThisCharacter(int _index)
    {
        currentIndex = _index;
        InstantiateCharacter();
    }

    public void SaveCharacterData()
    {
        foreach(CharacterCollectable characterCollectable in characterCollectables)
        {
            ES3.Save<bool>(characterCollectable.character.GetName() + "_isHaving", characterCollectable.isHaving);
        }

        ES3.Save<int>("characterIndex", currentIndex);
    }

    void LoadCharacterData()
    {
        foreach(CharacterCollectable characterCollectable in characterCollectables)
        {
            if(ES3.KeyExists(characterCollectable.character.GetName() + "_isHaving"))
            {
                characterCollectable.isHaving = ES3.Load<bool>(characterCollectable.character.GetName() + "_isHaving");
            }       
        }

        if(ES3.KeyExists("characterIndex")) currentIndex = ES3.Load<int>("characterIndex");     
    }

    public Character GetCharacter(int _index)
    {
        return characterCollectables[_index].character;
    }
}

[System.Serializable]
public class CharacterCollectable
{
    public Character character = null;
    public bool isHaving = false;
}