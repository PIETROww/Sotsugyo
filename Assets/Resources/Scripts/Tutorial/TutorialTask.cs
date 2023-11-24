using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TutorialTask
{
    //チュートリアルタイトル取得
    string GetTitle();
    //説明文取得
    string GetText();
    //チュートリアルタスクが設定された際に実行
    void OnTaskSetting();
    //チュートリアルタスクが達成されたか判定
    bool CheckTask();
    //達成後に次のタスクへ遷移するまでの時間（秒）
    float GetTransitionTime();
}
