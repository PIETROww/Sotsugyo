using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MutekiItemController : MonoBehaviour
{
    //Playerのスクリプトにコメントアウトした以下のソースコードをコピーすれば無敵の実装が出来ると思います

    //public float mutekiTime = 10.0f;
    //private float timeCnt = 0.0f;
    //private bool mutekiFlag = false;

    //void Update()
    //{
    //    //無敵フラグがtrueのときに実行する
    //    if (mutekiFlag)
    //    {
    //        Debug.Log("無敵状態");

    //        //無敵時間を進める
    //        timeCnt += Time.deltaTime;

    //        //無敵時間を過ぎたとき
    //        if (timeCnt >= mutekiTime)
    //        {
    //            Debug.Log("無敵状態終わり");

    //            //無敵フラグをfalseにする
    //            mutekiFlag = false;
    //            //無敵時間をリセットする
    //            timeCnt = 0.0f;
    //        }
    //    }
    //}

    //private void OnCollisionEnter(Collision other)
    //{
    //    //ぶつかった対象が無敵アイテムのタグの場合
    //    if (other.gameObject.tag == "MutekiItem")
    //    {
    //        //無敵フラグをtrueにする
    //        mutekiFlag = true;
    //    }

    //    //無敵アイテム効果時間中にぶつかった対象が敵のタグの場合、敵を倒す
    //    if (other.gameObject.tag == "Enemy")
    //    {
    //        if (mutekiFlag)
    //        {
    //            Destroy(other.gameObject);
    //        }
    //    }
    //}

    public float pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform.position.y;
    }

    // Update is called once per frame
    private void Update()
    {
        this.transform.position = new Vector3(transform.position.x, pos + Mathf.PingPong(Time.time, 1.0f), transform.position.z);
        this.transform.Rotate(0.0f, 1.0f, 0.0f);
    }

    //ぶつかった対象がPlayerだったら無敵アイテムを消滅させる
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
