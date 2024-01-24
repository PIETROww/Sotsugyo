using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameUIScript : MonoBehaviour
{
    private EventSystem eventSystem;
    GameManager gm;

    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject sousaUI;
    [SerializeField] private Button MenuButton;
    [SerializeField] private Button SousaButton;
    [SerializeField] private GameObject MenuPanel;

    [SerializeField] private Button ReturnButton;

    // Start is called before the first frame update
    void Start()
    {
        Menu.SetActive(false);
        sousaUI.SetActive(false);
        MenuPanel.SetActive(false);
        gm = GameManager.instance;
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        Debug.Log("TimeScale: " + Time.timeScale);
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
    public void MenuUI()
    {

        //�X�^�[�g���オ�O
        //�{�^����������else�łP�ɂȂ�
        //���̂܂܉������瓮���n�߂�
        //
        Menu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(MenuButton.gameObject);
    }
    public void MenuButtonDown()
    {
        if (Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
        MenuPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(SousaButton.gameObject);
    }

    public void SousaButtonDown()
    {
        sousaUI.SetActive(true);
        MenuPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(ReturnButton.gameObject);
    }

    public void ReturnButtonDown()
    {
        Time.timeScale = 1.0f;
        MenuPanel.SetActive(false);
        sousaUI.SetActive(false);
        MenuUI();
    }

    public void StageSelectButtonDown()
    {
        gm.LoadScene("StageSelect");
    }
}
