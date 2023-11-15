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

    //Player�̃X�N���v�g �����蔻��̉ӏ��ɒǉ�
    //if (other.gameObject.tag == "HealItem")
    //{
    //    //�񕜂��鏈��
    //}
}
