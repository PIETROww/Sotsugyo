using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Clear_flag : MonoBehaviour
{
    Animator animator;
   // private GameManager gameManager;
    AudioSource audioSource;
    [SerializeField] private AudioClip se1;
   [SerializeField] private Image clearImage;//UI‚Ì‰æ‘œ
    //Clear_move cm;
    // Start is called before the first frame update
    private void Start()
    {
        //clearImage = GameObject.Find("Clear").GetComponent<Image>();
        clearImage.enabled = false;//•\Ž¦‚©”ñ•\Ž¦‚©
        animator = GetComponent<Animator>();
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource=GetComponent<AudioSource>();
         //cm = GetComponent<Clear_move>();

       // animator.SetBool("Move", false);
        
    }
    
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("‚ ‚½‚Á‚½");
        if(other.gameObject.tag=="Player")
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
        SceneManager.LoadScene("Hozumi2");
    }
}
