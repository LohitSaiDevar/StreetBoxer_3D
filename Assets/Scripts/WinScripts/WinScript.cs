using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    void BreakDanceAnimation()
    {
        animator.SetTrigger("Breakdance");
    }
}
