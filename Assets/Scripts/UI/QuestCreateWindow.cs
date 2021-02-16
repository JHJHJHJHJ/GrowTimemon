using UnityEngine;

public class QuestCreateWindow : MonoBehaviour 
{
    public void CloseWindow() // 버튼에서 실행됨
    {
        this.gameObject.SetActive(false);
    }    
}