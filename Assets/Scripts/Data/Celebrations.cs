using UnityEngine;

[CreateAssetMenu(fileName = "Celebrations", menuName = "GrowTimemon/Celebrations", order = 0)]
public class Celebrations : ScriptableObject 
{
    [TextArea] public string[] celebrations = null; 

    public string GetRandomCelebration()
    {
        int randomIndex = Random.Range(0, celebrations.Length);

        return celebrations[randomIndex];
    }   
}