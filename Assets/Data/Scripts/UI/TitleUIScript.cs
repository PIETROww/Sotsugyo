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
    private GameUIScript gameUI;
    private TutorialManager tutorialM;

    private Animator animator;
    private EventSystem eventSystem;

    //タイトル画面の一番最初に出てくるボタンたち
    [SerializeField] private Button StartButton; //ゲームスタートアニメーション遷移ボタン
    [SerializeField] private Button CreditButton; //制作者一覧表＿表示ボタン
    [SerializeField] private Button QuitButton;　//ゲーム終了ボタン
    [SerializeField] private Button SettingButton; //設定ボタン

    //Setting（設定）ボタンが押された際に出てくるボタンたち
    [SerializeField] private Button TutorialButton; //チュートリアル遷移ボタン
    [SerializeField] private Button ReturnButton;　//戻るボタン
    [SerializeField] private GameObject MasterSlider;


    //ボタンが押された際の各種パネル（背景）たち
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

        //パネル・非表示
        SettingPanel.gameObject.SetActive(false);
        CreditPanel.gameObject.SetActive(false);
        SoundPanel.gameObject.SetActive(false);
        TutorialPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        //現在フォーカス（選択されている）ゲームオブジェクト取得
        //取得したゲームオブジェクトがボタンコンポーネントを所持しているか確認
        Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        //もしゲームパッドの東側のボタン（今回はBボタン）が押された際
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

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    //////////////////////////////////////////////////
    //                                              //
    // 　　　　　以下各種Onclickメソッド設定　　 　　  //
    //                                           　 //
    //////////////////////////////////////////////////

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void StartButtonDown()
    {
        animator.SetTrigger("GameStart");

        gameUI.Invoke("MenuUI", 7.5f);
        //つきのくんがカメラワークいじる
    }

    public void CreditButtonDown()
    {
        CreditPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(ReturnButton.gameObject);
    }

    public void QuitButtonDown()
    {
        //１．５秒後にゲームマネージャーのGameEnd関数遷移
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
