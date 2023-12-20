using System.Collections;
using UnityEngine;

public class PlayerSample : MonoBehaviour
{
    public float speed = 5f;
    public int HP = 3;
    float x, z;
    Rigidbody rb;
    Vector3 moving;

    //���G����
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


        //���G�t���O��true�̂Ƃ��Ɏ��s����
        if (mutekiFlag)
        {
            Debug.Log("���G���");

            //���G���Ԃ�i�߂�
            timeCnt += Time.deltaTime;

            //���G���Ԃ��߂����Ƃ�
            if (timeCnt >= mutekiTime)
            {
                Debug.Log("���G��ԏI���");

                //���G�t���O��false�ɂ���
                mutekiFlag = false;
                //���G���Ԃ����Z�b�g����
                timeCnt = 0.0f;
            }
        }
    }

    //�X�s�[�h�A�b�v
    IEnumerator SpeedUp()
    {
        speed = 7.0f;
        yield return new WaitForSeconds(10.0f);
        speed = 5.0f;
    }

    private void OnCollisionEnter(Collision other)
    {
        //�Ԃ������Ώۂ����G�A�C�e���̃^�O�̏ꍇ
        if (other.gameObject.tag == "MutekiItem")
        {
            //���G�t���O��true�ɂ���
            mutekiFlag = true;
        }

        //�Ԃ������Ώۂ��X�s�[�h�A�b�v�A�C�e���̏ꍇ
        if (other.gameObject.tag == "SpeedupItem")
        {
            StartCoroutine("SpeedUp");
        }

        //�Ԃ������Ώۂ��񕜃A�C�e���̏ꍇ
        if (other.gameObject.tag == "HealItem")
        {
            HP += 1;

            //HP��5�ȏ�񕜂��Ȃ��悤�ɂ���
            if (HP > 5)
            {
                HP = 5;
            }
        }

        //���G�A�C�e�����ʎ��Ԓ��ɂԂ������Ώۂ��G�̃^�O�̏ꍇ�A�G�����ł�����
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
