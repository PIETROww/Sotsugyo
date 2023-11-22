using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Clear_flag : MonoBehaviour
{
    Animator animator;
     private GameManager gameManager;
   [SerializeField] private Image clearImage;//UI‚Ì‰æ‘œ
    Clear_move cm;
    // Start is called before the first frame update
    private void Start()
    {
        clearImage = GameObject.Find("Clear").GetComponent<Image>();
        clearImage.enabled = false;//•\Ž¦‚©”ñ•\Ž¦‚©
        animator = GetComponent<Animator>();
        gameManager = GameManager.instance;
         cm = GetComponent<Clear_move>();

        animator.SetBool("Move", false);
        
    }
    
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("‚ ‚½‚Á‚½");
        if(other.gameObject.tag=="Player")
        {
            clearImage.enabled = true;
            cm.Animation();
        }
    }
   
    void SceneLoad()
    {
        gameManager.LoadScene("Hozumi2");
    }
}
