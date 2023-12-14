using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall2 : MonoBehaviour
{
    float defaultY;     // 扉の初期のY座標
    //float openY = 5f;   // 扉のオープン時のY座標
    //float speed = 1f;   // 扉の開閉のスピード

    public bool isOpen1; // 扉を開けるか閉めるかのフラグ
    public bool isOpen2; // 扉を開けるか閉めるかのフラグ
    void Start()
    {
        defaultY = transform.position.y;
    }

    void Update()
    {
        if (isOpen1 && isOpen2)
        {
            //transform.position += Vector3.up * speed * Time.deltaTime;
            Destroy(gameObject);
        }
    }
}
