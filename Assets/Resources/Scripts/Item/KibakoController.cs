using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class KibakoController : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject effectPrefab;
    //[SerializeField] private ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
       //ParticleSystem newParticle = Instantiate(particle);
        //newParticle.transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        //newParticle.Play();
        if (other.gameObject.tag == "Player")
        {
            GameObject destroyEffect = Instantiate(effectPrefab, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity);
            Destroy(destroyEffect, 3.0f);
            Destroy(this.gameObject);
            Instantiate(itemPrefab, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), itemPrefab.transform.rotation);
        }
    }
}

//Playerの砂煙のソースコード


//public GameObject effectPrefab;
//public GameObject effectPrefab2;
//public GameObject effectPrefab3;
//public GameObject effectPrefab4;
//public float t;


//if (this.state == State.Move)
//{
//    Effect();
//}


//private void Effect()
//{
//    t += Time.deltaTime;


//    if (t >= 0.5)
//    {
//        GameObject sunakemuriEffect = Instantiate(effectPrefab, new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), Quaternion.identity);
//        Destroy(sunakemuriEffect, 1.0f);
//        t = 0.0f;
//    }
//    if (this.state != State.Move)
//    {
//        t = 0.0f;
//    }
//}

//Playerの無敵アイテムとスピードアップアイテム取得時のエフェクト


////ぶつかった対象が無敵アイテムの場合
//if (other.gameObject.tag == "MutekiItem")
//{
//    mutekiFlag = true;
//    GameObject mutekiEffect = Instantiate(effectPrefab2, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
//    mutekiEffect.transform.parent = this.gameObject.transform;
//    Destroy(mutekiEffect, mutekiTime);

//    GameObject getEffect = Instantiate(effectPrefab4, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
//    //mutekiEffect.transform.parent = this.gameObject.transform;
//    Destroy(getEffect, 1.0f);
//}

////ぶつかった対象がスピードアップアイテムの場合
//if (other.gameObject.tag == "SpeedupItem")
//{
//    StartCoroutine("SpeedUp");
//    GameObject speedupEffect = Instantiate(effectPrefab3, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
//    speedupEffect.transform.parent = this.gameObject.transform;
//    Destroy(speedupEffect, mutekiTime);
//}