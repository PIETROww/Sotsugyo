using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Clear_flag2 : MonoBehaviour
{
    //Animator animator;
    // private GameManager gameManager;
    AudioSource audioSource;
    [SerializeField] private AudioClip se1;
    [SerializeField] private Image clearImage;//UIの画像
    //Clear_move cm;
    // Start is called before the first frame update
    private void Start()
    {
        //clearImage = GameObject.Find("Clear").GetComponent<Image>();
        clearImage.enabled = false;//表示か非表示か
        //animator = GetComponent<Animator>();
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        //cm = GetComponent<Clear_move>();

        // animator.SetBool("Move", false);

    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("あたった");
        if (other.gameObject.tag == "Player")
        {
            audioSource.PlayOneShot(se1);
            clearImage.enabled = true;
            //cm.Animation();

            Invoke("SceneLoad", 2);
        }
    }

    void SceneLoad()
    {
        clearImage.enabled = false;
        SceneManager.LoadScene("TitleScene");
    }
}
