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
    GameManager gameManager;

    [SerializeField] private Button StartButton;
    [SerializeField] private Button CreditButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private Button SettingButton;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        //���݃t�H�[�J�X����Ă���i�I������Ă���j�Q�[���I�u�W�F�N�g�擾
        //�擾�����Q�[���I�u�W�F�N�g���{�^���R���|�[�l���g�������Ă��邩�m�F
        Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            if (selectedButton != null)
            {
                selectedButton.onClick.Invoke();
            }
        }
    }


    /////////////////////////////////////////////////
    //                                             //
    // �@�@�@�@�@�ȉ��e��Onclick���\�b�h�ݒ�@�@ �@�@�@//
    //                                             //
    /////////////////////////////////////////////////
    
    public void StartButtonDown()
    {

    }

    public void CreditButtonDown()
    {

    }

    public void QuitButtonDown() 
    {
       gameManager.Invoke("GameEnd",1.5f);
    }

    public void SettingButtonDown()
    {
        
    }
}
