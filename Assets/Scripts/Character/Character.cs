using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Animator animator;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    public void InteractCharacter()
    {
        animator.SetTrigger("Interaction");
    }

    public void AnimateComplete()
    {
        animator.SetTrigger("Interaction");   
    }

    public void AnimateWork(bool isWorking)
    {
        animator.SetBool("isWorking", isWorking);
    }
}
