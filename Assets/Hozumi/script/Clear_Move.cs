using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear_move : MonoBehaviour
{
    Animator animator;
    Clear_flag clear;
    // private GameManager gameManager;
   

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Move", false);
    }
    // Update is called once per frame
    public void Animation()
     {
        animator.SetBool("Move", true);
        Debug.Log("Movetrue");
     }
}
