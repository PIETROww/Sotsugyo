using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealItemController : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
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
        this.transform.position = new Vector3(transform.position.x, pos + Mathf.PingPong(Time.time, 3.0f), transform.position.z);
        this.transform.Rotate(0.0f, 1.0f, 0.0f, Space.World);

        //�A�C�e�����Q�b�g�����Ƃ�
        if (get == true)
        {
            time += Time.deltaTime;

            //�Q�b�g�������̉�]���x�ύX
            this.transform.position = new Vector3(transform.position.x, transform.position.y + time * 5.0f, transform.position.z);
            this.transform.Rotate(0.0f, 15.0f, 0.0f, Space.World);

            //��莞�Ԍ���ł�����
            if (time > 0.5f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    //�Ԃ������Ώۂ�Player��������񕜃A�C�e�������ł�����
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (get == false)
            {
                ParticleSystem newParticle = Instantiate(particle);
                newParticle.transform.position = this.transform.position;
                newParticle.Play();

                bc.enabled = false;
                get = true;
            }
        }
    }

    //Player�̃X�N���v�g �����蔻��̉ӏ��ɒǉ�
    //if (other.gameObject.tag == "HealItem")
    //{
    //    //�񕜂��鏈��
    //}
}
