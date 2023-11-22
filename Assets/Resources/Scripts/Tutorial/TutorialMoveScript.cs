using UnityEngine;

public class TutorialMoveScript : TutorialTaskScript
{
   public string GetTitle()
    {
        return "基本動作　移動";
    }

    public string GetText()
    {
        return "WSADキーで上下左右に移動できます";
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
