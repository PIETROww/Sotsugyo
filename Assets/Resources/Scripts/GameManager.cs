using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int heartCount = 0;
    [SerializeField] private GameObject[] heart;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // for(int i=0;i<HP;++i)
        {
            heartCount++;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameEnd();
        }
    }

    public void Heart()
    {
        //その数に応じて表示非表示
        if (heartCount > 0)
        {
            heartCount--;

            // 対応するハートオブジェクトを非アクティブにする
            heart[heartCount].SetActive(false);
        }

        if (heartCount == 0)
        {
            //GameOver();
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
