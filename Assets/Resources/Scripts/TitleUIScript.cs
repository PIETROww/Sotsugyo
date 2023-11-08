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
        //現在フォーカスされている（選択されている）ゲームオブジェクト取得
        //取得したゲームオブジェクトがボタンコンポーネント所持しているか確認
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
    // 　　　　　以下各種Onclickメソッド設定　　 　　　//
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
