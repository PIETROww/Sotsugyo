using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class Manual : MonoBehaviour
{
    [SerializeField] private GameObject[] Panel;
    [SerializeField] private int count = 0;
    private
        int pre;

    // Start is called before the first frame update
    void Start()
    {
        //�p�l���ꖇ�ڈȊO�S����\��
        for (int i = 0; i < Panel.Length; i++)
        {
            Panel[i].SetActive(false);
        }

        // �����\���Ƃ��� count �̒l�̃p�l����\��
        Panel[count].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.dpad.left.wasReleasedThisFrame)
        {
            pre--;
        }
        else if (Gamepad.current.dpad.right.wasReleasedThisFrame)
        {
            pre++;
        }
        // ���[�v����悤�ɒ���
        pre = (pre + Panel.Length) % Panel.Length;

        count = pre;

        // �S�Ẵp�l�����\���ɂ��Acount �̒l�̃p�l��������\��
        for (int i = 0; i < Panel.Length; i++)
        {
            Panel[i].SetActive(i == count);
        }
    }
}

