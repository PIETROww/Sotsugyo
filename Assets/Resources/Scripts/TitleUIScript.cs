using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//B�{�^���������ꂽ��A�j���[�V�����Ń^�C�g���E�{�^����ʊO��
//���̂܂܃Q�[���X�^�[�g
//�^�C�g���Ŏg�p���ꂽ�{�^�������I�t

public class TitleUIScript : MonoBehaviour
{
    private GameManager gameManager;
    public Animator animator;
    private EventSystem eventSystem;

    [SerializeField] private Button StartButton;
    [SerializeField] private Button CreditButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private Button SettingButton;

    [SerializeField] private GameObject SettingPanel;
    [SerializeField] private GameObject CreditPanel;
    
    [SerializeField] private Button TutorialButton;
    [SerializeField] private Button ReturnButton;

    void Start()
    {
        gameManager = GameManager.instance;
        animator = gameObject.GetComponent<Animator>();
        eventSystem = EventSystem.current;
        SettingPanel.gameObject.SetActive(false);
        CreditPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        //���݃t�H�[�J�X�i�I������Ă���j�Q�[���I�u�W�F�N�g�擾
        //�擾�����Q�[���I�u�W�F�N�g���{�^���R���|�[�l���g���������Ă��邩�m�F
        Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            if (selectedButton != null)
            {
                selectedButton.onClick.Invoke();
            }
        }
    }

    
    //////////////////////////////////////////////////
    //                                              //
    // �@�@�@�@�@�ȉ��e��Onclick���\�b�h�ݒ�@�@ �@�@  //
    //                                           �@ //
    //////////////////////////////////////////////////

    public void StartButtonDown()
    {
        animator.SetTrigger("GameStart");
       

        //���̂��񂪃J�������[�N������
    }

    public void CreditButtonDown()
    {
        CreditPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(ReturnButton.gameObject);
    }

    public void QuitButtonDown()
    {
        gameManager.Invoke("GameEnd", 1.5f);
    }

    public void SettingButtonDown()
    {
        SettingPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(TutorialButton.gameObject);
    }

    public void TutorialButtonDown()
    {

    }

    public void SoundButtonDown()
    {

    }

    public void NullButtonDown()
    {

    }

    public void ReturnButtonDown()
    {
        SettingPanel.gameObject.SetActive(false);
        CreditPanel.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(StartButton.gameObject);
    }
}
