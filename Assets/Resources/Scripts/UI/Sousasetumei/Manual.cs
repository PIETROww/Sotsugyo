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
        //パネル一枚目以外全部非表示
        for (int i = 0; i < Panel.Length; i++)
        {
            Panel[i].SetActive(false);
        }

        // 初期表示として count の値のパネルを表示
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
        // ループするように調整
        pre = (pre + Panel.Length) % Panel.Length;

        count = pre;

        // 全てのパネルを非表示にし、count の値のパネルだけを表示
        for (int i = 0; i < Panel.Length; i++)
        {
            Panel[i].SetActive(i == count);
        }
    }
}

