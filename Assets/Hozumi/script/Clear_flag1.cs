using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Clear_flag1 : MonoBehaviour
{
   
    AudioSource audioSource;
    [SerializeField] private AudioClip se1;
   [SerializeField] private Image clearImage;//UI�̉摜
    //Clear_move cm;
    // Start is called before the first frame update
    private void Start()
    {
        clearImage.enabled = false;//�\������\����
        audioSource=GetComponent<AudioSource>();
    }
    
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("��������");
        if(other.gameObject.tag=="Player")
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
