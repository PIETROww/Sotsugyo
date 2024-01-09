using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TutorialTask
{
    //�`���[�g���A���^�C�g���擾
    string GetTitle();
    //�������擾
    string GetText();
    //�`���[�g���A���^�X�N���ݒ肳�ꂽ�ۂɎ��s
    void OnTaskSetting();
    //�`���[�g���A���^�X�N���B�����ꂽ������
    bool CheckTask();
    //�B����Ɏ��̃^�X�N�֑J�ڂ���܂ł̎��ԁi�b�j
    float GetTransitionTime();
}
