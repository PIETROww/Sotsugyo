using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAttack : TutorialTask
{
    public string GetTitle()
    {
        return "基本操作 攻撃 (1/2)";
    }

    public string GetText()
    {
        return "左クリックで杖から星弾を発射して攻撃します。" + Environment.NewLine + "押し続けると連続発射します。";
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
