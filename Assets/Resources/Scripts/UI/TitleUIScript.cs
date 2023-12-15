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
    private GameUIScript gameUI;
    private TutorialManager tutorialM;

    private Animator animator;
    private EventSystem eventSystem;

    //�^�C�g����ʂ̈�ԍŏ��ɏo�Ă���{�^������
    [SerializeField] private Button StartButton; //�Q�[���X�^�[�g�A�j���[�V�����J�ڃ{�^��
    [SerializeField] private Button CreditButton; //����҈ꗗ�\�Q�\���{�^��
    [SerializeField] private Button QuitButton;�@//�Q�[���I���{�^��
    [SerializeField] private Button SettingButton; //�ݒ�{�^��

    //Setting�i�ݒ�j�{�^���������ꂽ�ۂɏo�Ă���{�^������
    [SerializeField] private Button TutorialButton; //�`���[�g���A���J�ڃ{�^��
    [SerializeField] private Button ReturnButton;�@//�߂�{�^��
    [SerializeField] private GameObject MasterSlider;


    //�{�^���������ꂽ�ۂ̊e��p�l���i�w�i�j����
    [SerializeField] private GameObject SettingPanel;
    [SerializeField] private GameObject CreditPanel;
    [SerializeField] private GameObject SoundPanel;
    [SerializeField] private GameObject TutorialPanel;

  
    void Start()
    {
        gameManager = GameManager.instance;
        gameUI=GetComponent<GameUIScript>();
        tutorialM=GetComponent<TutorialManager>();
        animator = gameObject.GetComponent<Animator>();
        eventSystem = EventSystem.current;

        //�p�l���E��\��
        SettingPanel.gameObject.SetActive(false);
        CreditPanel.gameObject.SetActive(false);
        SoundPanel.gameObject.SetActive(false);
        TutorialPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        //���݃t�H�[�J�X�i�I������Ă���j�Q�[���I�u�W�F�N�g�擾
        //�擾�����Q�[���I�u�W�F�N�g���{�^���R���|�[�l���g���������Ă��邩�m�F
        Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        //�����Q�[���p�b�h�̓����̃{�^���i�����B�{�^���j�������ꂽ��
        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            if (selectedButton != null)
            {
                selectedButton.onClick.Invoke();
            }
        }

        if (TutorialPanel==true)
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                TutorialPanel.gameObject.SetActive(false);
            }
        }
    }

    //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
    //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

    //////////////////////////////////////////////////
    //                                              //
    // �@�@�@�@�@�ȉ��e��Onclick���\�b�h�ݒ�@�@ �@�@  //
    //                                           �@ //
    //////////////////////////////////////////////////

    //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
    //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
    public void StartButtonDown()
    {
        animator.SetTrigger("GameStart");

        gameUI.Invoke("MenuUI", 7.5f);
        //���̂��񂪃J�������[�N������
    }

    public void CreditButtonDown()
    {
        CreditPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(ReturnButton.gameObject);
    }

    public void QuitButtonDown()
    {
        //�P�D�T�b��ɃQ�[���}�l�[�W���[��GameEnd�֐��J��
        gameManager.Invoke("GameEnd", 1.5f);
    }

    public void SettingButtonDown()
    {
        SettingPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(TutorialButton.gameObject);
    }

    public void TutorialButtonDown()
    {
        TutorialPanel.gameObject.SetActive(true);
        tutorialM.SwitchEnabled();
    }

    public void SoundButtonDown()
    {
        SoundPanel.gameObject.SetActive(true);
        SettingPanel.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(MasterSlider.gameObject);
    }

    public void NullButtonDown()
    {

    }

    public void ReturnButtonDown()
    {
        SettingPanel.gameObject.SetActive(false);
        CreditPanel.gameObject.SetActive(false);
        SoundPanel.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(StartButton.gameObject);
    }
}
