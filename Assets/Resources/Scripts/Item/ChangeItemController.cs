using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeItemController : MonoBehaviour
{
    //[SerializeField] private ParticleSystem particle;
    public GameObject effectPrefab;
    public float pos;
    private float time;
    private bool get = false;
    private BoxCollider bc;

    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform.position.y;
        bc = this.gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(transform.position.x, pos + Mathf.PingPong(Time.time, 1.0f), transform.position.z);
        this.transform.Rotate(0.0f, 3.0f, 0.0f, Space.World);

        //アイテムをゲットしたとき
        if (get == true)
        {
            time += Time.deltaTime;

            //ゲットした時の回転速度変更
            this.transform.position = new Vector3(transform.position.x, transform.position.y + time * 4.0f, transform.position.z);
            this.transform.Rotate(0.0f, 15.0f, 0.0f, Space.World);

            //一定時間後消滅させる
            if (time > 0.5f)
            {
                //ParticleSystem newParticle = Instantiate(particle);
                //newParticle.transform.position = this.transform.position;
                //newParticle.Play();

                GameObject itemgetEffect = Instantiate(effectPrefab, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity);
                Destroy(itemgetEffect, 3.0f);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (get == false)
            {
                bc.enabled = false;
                get = true;
            }
        }
    }
}
