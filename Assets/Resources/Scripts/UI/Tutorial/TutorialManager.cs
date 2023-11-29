using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using DG.Tweening;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    // チュートリアル用UI
    protected RectTransform TutorialTextArea;
    protected TextMeshProUGUI TutorialTitle;
    protected TextMeshProUGUI TutorialText;

    // チュートリアルタスク
    protected TutorialTask currentTask;
    protected List<TutorialTask> tutorialTask;

    // チュートリアル表示フラグ
    private bool isEnabled;

    // チュートリアルタスクの条件を満たした際の遷移用フラグ
    private bool task_executed = false;

    // チュートリアル表示時のUI移動距離
    private float fade_pos_x = 350;

    void Awake()
    {
        // チュートリアル表示用UIのインスタンス取得
        TutorialTextArea = GameObject.Find("TutorialTextArea").GetComponent<RectTransform>();
        TutorialTitle = TutorialTextArea.Find("Title").GetComponentInChildren<TextMeshProUGUI>();
        TutorialText = TutorialTextArea.Find("Text").GetComponentInChildren<TextMeshProUGUI>();

        // チュートリアルの一覧
        tutorialTask = new List<TutorialTask>()
        {
    new TutorialMove(),
    new TutorialAttack(),
        };

        // 最初のチュートリアルを設定
        StartCoroutine(SetCurrentTask(tutorialTask.First()));
        isEnabled = true;
    }

    void Update()
    {
        // チュートリアルが存在し実行されていない場合に処理
        if (currentTask != null && !task_executed)
        {
            // 現在のチュートリアルが実行されたか判定
            if (currentTask.CheckTask())
            {
                task_executed = true;

                DOVirtual.DelayedCall(currentTask.GetTransitionTime(), () => {
                    iTween.MoveTo(TutorialTextArea.gameObject, iTween.Hash(
                        "position", TutorialTextArea.transform.position + new Vector3(fade_pos_x, 0, 0),
                        "time", 1f
                    ));

                    tutorialTask.RemoveAt(0);

                    var nextTask = tutorialTask.FirstOrDefault();
                    if (nextTask != null)
                    {
                        StartCoroutine(SetCurrentTask(nextTask, 1f));
                    }
                });
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            SwitchEnabled();
        }
    }
    //新しいチュートリアルタスク設定
    protected IEnumerator SetCurrentTask(TutorialTask task, float time = 0)
    {
        // timeが指定されている場合は待機
        yield return new WaitForSeconds(time);

        currentTask = task;
        task_executed = false;

        // UIにタイトルと説明文を設定
        TutorialTitle.text = task.GetTitle();
        TutorialText.text = task.GetText();

        // チュートリアルタスク設定時用の関数を実行
        task.OnTaskSetting();

        iTween.MoveTo(TutorialTextArea.gameObject, iTween.Hash(
            "position", TutorialTextArea.transform.position - new Vector3(fade_pos_x, 0, 0),
            "time", 1f
        ));
    }

    //チュートリアルの有効無効切り替え
    protected void SwitchEnabled()
    {
        isEnabled = !isEnabled;

        // UIの表示切り替え
        float alpha = isEnabled ? 1f : 0;
        TutorialTextArea.GetComponent<CanvasGroup>().alpha = alpha;
    }
}
