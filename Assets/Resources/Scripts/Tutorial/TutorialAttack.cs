using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAttack : TutorialTask
{
    public string GetTitle()
    {
        return "��{���� �U�� (1/2)";
    }

    public string GetText()
    {
        return "���N���b�N�ŏ񂩂琯�e�𔭎˂��čU�����܂��B" + Environment.NewLine + "����������ƘA�����˂��܂��B";
    }

    public void OnTaskSetting()
    {
    }

    public bool CheckTask()
    {
        if (Input.GetButton("Attack"))
        {
            return true;
        }

        return false;
    }

    public float GetTransitionTime()
    {
        return 2f;
    }
}
