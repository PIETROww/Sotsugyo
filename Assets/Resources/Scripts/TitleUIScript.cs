using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Bボタンが押されたらアニメーションでタイトル・ボタン画面外へ
//そのままゲームスタート
//タイトルで使用されたボタンたちオフ

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
        //現在フォーカス（選択されている）ゲームオブジェクト取得
        //取得したゲームオブジェクトがボタンコンポーネントを所持しているか確認
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
    // 　　　　　以下各種Onclickメソッド設定　　 　　  //
    //                                           　 //
    //////////////////////////////////////////////////

    public void StartButtonDown()
    {
        animator.SetTrigger("GameStart");
       

        //つきのくんがカメラワークいじる
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
