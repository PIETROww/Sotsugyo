using UnityEngine;

public class TutorialMoveScript : TutorialTaskScript
{
   public string GetTitle()
    {
        return "��{����@�ړ�";
    }

    public string GetText()
    {
        return "WSAD�L�[�ŏ㉺���E�Ɉړ��ł��܂�";
    }

    public void OnTaskSetting()
    {

    }


    public bool CheckTask()
    {
        float axis_h = Input.GetAxis("Horizontal");
        float axis_v = Input.GetAxis("Vertical");

        if(0<axis_v||0<axis_h)
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
