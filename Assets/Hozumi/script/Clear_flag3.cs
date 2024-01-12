using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Clear_flag3 : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private AudioClip se1;
    [SerializeField] private Image clearImage;//UI�̉摜

    // Start is called before the first frame update
    private void Start()
    {
        clearImage.enabled = false;//�\������\����  
        audioSource = GetComponent<AudioSource>();

    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("��������");
        if (other.gameObject.tag == "Player")
        {
            audioSource.PlayOneShot(se1);
            clearImage.enabled = true;

            Invoke("SceneLoad", 2);
        }
    }

    void SceneLoad()
    {
        clearImage.enabled = false;
        SceneManager.LoadScene("StageSelect");
    }
}
