using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Clear_Move : MonoBehaviour
{
    private Animator animator;
    private GameManager gameManager;

    // Start is called before the first frame update
    private void Start()
    {
        animator= GetComponent<Animator>();
        gameManager = GameManager.instance;
        animator.SetBool("Move", false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        animator.SetBool("Move", true);
        Invoke("SceneLoad", 5);
    }
    private void SceneLoad()
    {
        gameManager.LoadScene("Hozumi2");
    }
}
