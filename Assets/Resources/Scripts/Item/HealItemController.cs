using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealItemController : MonoBehaviour
{
    public float pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(transform.position.x, pos + Mathf.PingPong(Time.time, 3.0f), transform.position.z);
        this.transform.Rotate(0.0f, 1.0f, 0.0f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }

    //Playerのスクリプト 当たり判定の箇所に追加
    //if (other.gameObject.tag == "HealItem")
    //{
    //    //回復する処理
    //}
}
