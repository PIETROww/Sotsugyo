using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Clear_Move : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
        animator.SetBool("Move", false);
    }

   //�L�����N�^�[�ɂԂ�������A�j���[�V��������
    void OnCollisionEnter(Collision collision)
    { //
        if (collision.gameObject.name == "Player")
        {
            animator.SetBool("Move", true);
        }
    }
}
