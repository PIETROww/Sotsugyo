
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    // �`���[�g���A���pUI
    protected RectTransform TutorialTextArea;
    protected TextMeshProUGUI TutorialTitle;
    protected TextMeshProUGUI TutorialText;

    // �`���[�g���A���^�X�N
    protected TutorialTaskScript currentTask;
    protected List<TutorialTaskScript> tutorialTask;

    // �`���[�g���A���\���t���O
    private bool isEnabled;

    // �`���[�g���A���^�X�N�̏����𖞂������ۂ̑J�ڗp�t���O
    private bool task_executed = false;

    // �`���[�g���A���\������UI�ړ�����
    private float fade_pos_x = 350;

    void Start()
    {
        // �`���[�g���A���\���pUI�̃C���X�^���X�擾
        TutorialTextArea = GameObject.Find("TutorialTextArea").GetComponent<RectTransform>();
        TutorialTitle = TutorialTextArea.Find("Title").GetComponentInChildren<TextMeshProUGUI>();
        TutorialText = TutorialTextArea.Find("Text").GetComponentInChildren<TextMeshProUGUI>();

        // �`���[�g���A���̈ꗗ
        tutorialTask = new List<TutorialTaskScript>()
        {
    new TutorialMoveScript(),
    new TutorialAttackScript(),
        };

        // �ŏ��̃`���[�g���A����ݒ�
        StartCoroutine(SetCurrentTask(tutorialTask.First()));

        isEnabled = true;
    }

    void Update()
    {
        // �`���[�g���A�������݂����s����Ă��Ȃ��ꍇ�ɏ���
        if (currentTask != null && !task_executed)
        {
            // ���݂̃`���[�g���A�������s���ꂽ������
            if (currentTask.CheckTask())
            {
                task_executed = true;

                DOVirtual.DelayedCall(currentTask.GetTransitionTime(), () =>
                {
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


    //�V�����`���[�g���A���^�X�N�ݒ�
    protected IEnumerator SetCurrentTask(TutorialTaskScript task, float time = 0)
    {
        // time���w�肳��Ă���ꍇ�͑ҋ@
        yield return new WaitForSeconds(time);

        currentTask = task;
        task_executed = false;

        // UI�Ƀ^�C�g���Ɛ�������ݒ�
        TutorialTitle.text = task.GetTitle();
        TutorialText.text = task.GetText();

        // �`���[�g���A���^�X�N�ݒ莞�p�̊֐������s
        task.OnTaskSetting();

        iTween.MoveTo(TutorialTextArea.gameObject, iTween.Hash(
            "position", TutorialTextArea.transform.position - new Vector3(fade_pos_x, 0, 0),
            "time", 1f
        ));
    }

    //�`���[�g���A���̗L�������؂�ւ�
    protected void SwitchEnabled()
    {
        isEnabled = !isEnabled;

        // UI�̕\���؂�ւ�
        float alpha = isEnabled ? 1f : 0;
        TutorialTextArea.GetComponent<CanvasGroup>().alpha = alpha;
    }
}
