using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutekiItemController : MonoBehaviour
{
    //Player�̃X�N���v�g�ɃR�����g�A�E�g�����ȉ��̃\�[�X�R�[�h���R�s�[����Ζ��G�̎������o����Ǝv���܂�

    //public float mutekiTime = 10.0f;
    //private float timeCnt = 0.0f;
    //private bool mutekiFlag = false;

    //void Update()
    //{
    //    //���G�t���O��true�̂Ƃ��Ɏ��s����
    //    if (mutekiFlag)
    //    {
    //        Debug.Log("���G���");

    //        //���G���Ԃ�i�߂�
    //        timeCnt += Time.deltaTime;

    //        //���G���Ԃ��߂����Ƃ�
    //        if (timeCnt >= mutekiTime)
    //        {
    //            Debug.Log("���G��ԏI���");

    //            //���G�t���O��false�ɂ���
    //            mutekiFlag = false;
    //            //���G���Ԃ����Z�b�g����
    //            timeCnt = 0.0f;
    //        }
    //    }
    //}

    //private void OnCollisionEnter(Collision other)
    //{
    //    //�Ԃ������Ώۂ����G�A�C�e���̃^�O�̏ꍇ
    //    if (other.gameObject.tag == "MutekiItem")
    //    {
    //        //���G�t���O��true�ɂ���
    //        mutekiFlag = true;
    //    }

    //    //���G�A�C�e�����ʎ��Ԓ��ɂԂ������Ώۂ��G�̃^�O�̏ꍇ�A�G��|��
    //    if (other.gameObject.tag == "Enemy")
    //    {
    //        if (mutekiFlag)
    //        {
    //            Destroy(other.gameObject);
    //        }
    //    }
    //}


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    //�Ԃ������Ώۂ�Player�������疳�G�A�C�e�������ł�����
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
