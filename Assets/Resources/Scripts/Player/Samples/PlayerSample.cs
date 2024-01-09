using System.Collections;
using UnityEngine;

public class PlayerSample : MonoBehaviour
{
    public float speed = 5f;
    public int HP = 3;
    float x, z;
    Rigidbody rb;
    Vector3 moving;

    //無敵時間
    public float mutekiTime = 10.0f;
    private float timeCnt = 0.0f;
    private bool mutekiFlag = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        moving = new Vector3(x, 0, z);

        moving = moving.normalized * speed;

        rb.velocity = moving;


        //無敵フラグがtrueのときに実行する
        if (mutekiFlag)
        {
            Debug.Log("無敵状態");

            //無敵時間を進める
            timeCnt += Time.deltaTime;

            //無敵時間を過ぎたとき
            if (timeCnt >= mutekiTime)
            {
                Debug.Log("無敵状態終わり");

                //無敵フラグをfalseにする
                mutekiFlag = false;
                //無敵時間をリセットする
                timeCnt = 0.0f;
            }
        }
    }

    //スピードアップ
    IEnumerator SpeedUp()
    {
        speed = 7.0f;
        yield return new WaitForSeconds(10.0f);
        speed = 5.0f;
    }

    private void OnCollisionEnter(Collision other)
    {
        //ぶつかった対象が無敵アイテムのタグの場合
        if (other.gameObject.tag == "MutekiItem")
        {
            //無敵フラグをtrueにする
            mutekiFlag = true;
        }

        //ぶつかった対象がスピードアップアイテムの場合
        if (other.gameObject.tag == "SpeedupItem")
        {
            StartCoroutine("SpeedUp");
        }

        //ぶつかった対象が回復アイテムの場合
        if (other.gameObject.tag == "HealItem")
        {
            HP += 1;

            //HPが5以上回復しないようにする
            if (HP > 5)
            {
                HP = 5;
            }
        }

        //無敵アイテム効果時間中にぶつかった対象が敵のタグの場合、敵を消滅させる
        if (other.gameObject.tag == "Enemy")
        {
            HP -= 1;

            if (mutekiFlag)
            {
                Destroy(other.gameObject);
            }
            if (HP == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
