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
        //���̐��ɉ����ĕ\����\��
        if (heartCount > 0)
        {
            heartCount--;

            // �Ή�����n�[�g�I�u�W�F�N�g���A�N�e�B�u�ɂ���
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
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
    }
}
