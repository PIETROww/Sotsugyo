using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum State
    {
        Idle,
        Move,
        Jump,
        Attack,
        Damage,
        Dead,
    }
    private State state;

    private CharaUniqueAction uniqueAction = null;

    private float inputX, inputZ;
    private Rigidbody rb;
    private Vector3 moveForward;

    [SerializeField] private Animator animator;
    private GameObject attackObj;
    public GameObject catAttackObj;
    public GameObject duckAttackObj;
    public GameObject penguinAttackObj;
    public GameObject sheepAttackObj;

    //�X�e�[�^�X--------------------------
    public int HP = 5;
    public int ATK = 1;
    public float speed = 5f;

    //JumpSample.cs-----------------------
    [SerializeField, Min(0)]
    float jumpPower = 5f;
    [SerializeField]
    AnimationCurve jumpCurve = new();
    [SerializeField, Min(0)]
    float maxJumpTime = 1f;
    [SerializeField]
    float groundCheckRadius = 0.4f;
    [SerializeField]
    float groundCheckOffsetY = 0.07f;
    [SerializeField]
    float groundCheckDistance = 0.2f;
    [SerializeField]
    LayerMask groundLayers = 0;
    [SerializeField]
    string JumpButtonName = "Jump";

    private bool isGrounded = false;
    private bool jumping = false;
    private float jumpTime = 0;
    private RaycastHit hit;
    private Transform tr;

    //�U��--------------------------------
    //public GameObject attackRange;
    public float attackTime = 1.0f;
    private float attackCnt = 0.0f;
    private bool attackFlag = true;

    //�_���[�W���󂯂��Ƃ��̏���----------
    public float damageTime = 2.0f;
    private float damageCnt = 0.0f;

    //���G--------------------------------
    public float mutekiTime = 10.0f;
    private float timeCnt = 0.0f;
    private bool mutekiFlag = false;

    //�ϐg--------------------------------
    public GameObject[] characters;
    bool catFlag = false,
        duckFlag = false,
        penguinFlag = true,     //�ŏ��̓y���M���ɂ��Ă���
        sheepFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = this.transform;

        //Copy(ref uniqueAction, ref characters, ref animator, ref attackObj,
        //    catFlag, duckFlag, penguinFlag, sheepFlag);
        Copy();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");


        //��������W�����v
        isGrounded = CheckGroundStatus();

        // �W�����v�̊J�n����
        if (isGrounded && Input.GetButtonDown(JumpButtonName))
        {
            jumping = true;
        }
        // �W�����v���̏���
        if (jumping)
        {
            if (Input.GetButtonUp(JumpButtonName) || jumpTime >= maxJumpTime)
            {
                jumping = false;
                jumpTime = 0;
            }
            else if (Input.GetButton(JumpButtonName))
            {
                jumpTime += Time.deltaTime;
            }
        }

        //�m�F
        //Debug.Log(moveForward);
        //Debug.Log(isGrounded);
        //Debug.Log(this.state);

        //�X�t�B�A�L���X�g�̊m�F�p
        //var start = transform.position + groundCheckOffsetY * Vector3.up;
        //var end = transform.position + groundCheckOffsetY * Vector3.up + groundCheckDistance * Vector3.down;
        //Debug.DrawLine(start, end, Color.red);
        //Debug.Log("start:" + start + " end:" + end);
    }


    private void FixedUpdate()
    {
        //�J�����̌�������ɂ������ʂ̃x�N�g��
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        //moving = new Vector3(inputX, 0, inputZ);
        //moving = moving.normalized;

        //�J������̓���
        moveForward = cameraForward * inputZ + Camera.main.transform.right * inputX;
        moveForward = moveForward.normalized;

        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }

        //��ԑJ��
        StateThink();
        //��Ԗ��̏���
        StateMove();
        //���G���
        Muteki();
    }

    private void StateThink()
    {
        switch (this.state)
        {
            case State.Idle:
                if (isGrounded && moveForward.magnitude > 0) { this.state = State.Move; }
                if (jumping) { this.state = State.Jump; }
                if (Input.GetKeyDown(KeyCode.N)) { this.state = State.Attack; }
                //�_���[�W�ւ̑J��
                break;
            case State.Move:
                if (moveForward.magnitude <= 0) { this.state = State.Idle; }
                if (jumping) { this.state = State.Jump; }
                if (Input.GetKeyDown(KeyCode.N)) { this.state = State.Attack; }
                //�_���[�W�ւ̑J��
                break;
            case State.Jump:
                if (isGrounded) { this.state = State.Idle; }
                if (Input.GetKeyDown(KeyCode.N))
                {
                    //if(action is PlayerUniqueActionPenguin actionPenguin)
                    //{
                    //    actionPenguin.oneShot = false;
                    //}
                    this.state = State.Attack;

                }
                //�_���[�W�ւ̑J��
                break;

            case State.Attack:
                if (attackTime <= attackCnt)
                {
                    attackCnt = 0.0f;
                    attackFlag = true;
                    this.state = State.Idle;
                }
                break;
            case State.Damage:
                if (damageTime <= damageCnt)
                {
                    damageCnt = 0.0f;
                    this.state = State.Idle;
                }
                if (this.HP <= 0) { this.state = State.Dead; }
                break;
        }
    }

    void StateMove()
    {
        switch (this.state)
        {
            case State.Idle:
                animator.SetTrigger("Idle");
                break;
            case State.Move:
                animator.SetTrigger("Walk");
                Move();
                break;
            case State.Jump:
                animator.SetTrigger("jump");
                Move();
                Jump();
                break;
            case State.Attack:
                Move();         //�����Ȃ���U���ł���悤�ɂ���
                attackCnt += Time.deltaTime;
                if (attackFlag && 0.5f <= attackCnt)
                {
                    Attack();
                    attackFlag = false;
                }
                break;
            case State.Damage:
                animator.SetTrigger("stun");
                damageCnt += Time.deltaTime;
                Damage();
                break;
            case State.Dead:
                Dead();
                break;
        }
    }

    private void Move()
    {
        tr.position += moveForward * speed * Time.deltaTime;
    }
    void Jump()
    {
        if (!jumping)
        {
            return;
        }
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        // �W�����v�̑��x���A�j���[�V�����J�[�u����擾
        float t = jumpTime / maxJumpTime;
        float power = jumpPower * jumpCurve.Evaluate(t);
        if (t >= 1)
        {
            jumping = false;
            jumpTime = 0;
        }
        rb.AddForce(power * Vector3.up, ForceMode.Impulse);
    }

    void Attack()
    {
        uniqueAction.Action(attackObj, animator, attackCnt);
    }

    void Damage()
    {

    }

    void Dead()
    {

    }

    private void Muteki()
    {
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

    void Falling()
    {
        if (transform.position.y <= -10)
        {
            //�J�������Ƀ`�F�b�N�|�C���g�t�߂Ɉړ�������

            //�`�F�b�N�|�C���g�Ƀ��[�v����

            //���͂Ƃ肠�����Q�[���I�[�o�[��
            HP = 0;
        }
    }

    //�ϐg
    //public void Copy(ref CharaUniqueAction uniqueAction, ref GameObject[] characters, ref Animator animator, ref GameObject attackObj,
    //    bool catFlag, bool duckFlag, bool penguinFlag, bool sheepFlag)  //����������
    public void Copy()
    {

        if (uniqueAction != null)
        {
            Destroy(uniqueAction);  //��x�\�͂�����
            uniqueAction = null;    //null������
        }

        if (catFlag)
        {
            characters[0].SetActive(true);
            characters[1].SetActive(false);
            characters[2].SetActive(false);
            characters[3].SetActive(false);
            animator = characters[0].GetComponent<Animator>();
            attackObj = catAttackObj;
            uniqueAction = gameObject.AddComponent<CharaUniqueActionCat>();
        }
        if (duckFlag)
        {
            characters[0].SetActive(false);
            characters[1].SetActive(true);
            characters[2].SetActive(false);
            characters[3].SetActive(false);
            animator = characters[1].GetComponent<Animator>();
            attackObj = duckAttackObj;
            uniqueAction = gameObject.AddComponent<CharaUniqueActionDuck>();
        }
        if (penguinFlag)
        {
            characters[0].SetActive(false);
            characters[1].SetActive(false);
            characters[2].SetActive(true);
            characters[3].SetActive(false);
            animator = characters[2].GetComponent<Animator>();
            attackObj = penguinAttackObj;
            uniqueAction = gameObject.AddComponent<CharaUniqueActionPenguin>();
        }
        if (sheepFlag)
        {
            characters[0].SetActive(false);
            characters[1].SetActive(false);
            characters[2].SetActive(false);
            characters[3].SetActive(true);
            animator = characters[3].GetComponent<Animator>();
            attackObj = sheepAttackObj;
            uniqueAction = gameObject.AddComponent<CharaUniqueActionSheep>();
        }
    }

    bool CheckGroundStatus()
    {
        bool isHit = false;
        if (Physics.SphereCast(
            tr.position + groundCheckOffsetY * Vector3.up,
            groundCheckRadius,
            Vector3.down,
            out hit,
            groundCheckDistance,
            groundLayers,
            QueryTriggerInteraction.Ignore
            ))
        {
            isHit = true;
        }
        return isHit;
    }

    //���n�������������邽�߂̏���
    private void OnDrawGizmos()
    {
        var start = transform.position + groundCheckOffsetY * Vector3.up;
        var end = transform.position + groundCheckOffsetY * Vector3.up + groundCheckDistance * Vector3.down;

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        float radius = Mathf.Max(0.01f, groundCheckRadius);
        for (float y = start.y; y >= end.y; y -= radius)
        {
            Gizmos.DrawSphere(new Vector3(start.x, y, start.z), groundCheckRadius);
            //Debug.Log("gizmo y:" + y);
        }

        //Gizmos.DrawSphere(Vector3.zero, 100);
    }


    public GameObject[] copyItem;
    private void OnCollisionEnter(Collision other)
    {
        //�Ԃ������Ώۂ����G�A�C�e���̏ꍇ
        if (other.gameObject.tag == "MutekiItem")
            mutekiFlag = true;

        //�Ԃ������Ώۂ��X�s�[�h�A�b�v�A�C�e���̏ꍇ
        if (other.gameObject.tag == "SpeedupItem")
            StartCoroutine("SpeedUp");

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

        if (other.gameObject.tag == "Enemy")
        {
            ////���G�̎��́A�G��|��
            //if (mutekiFlag)
            //    Destroy(other.gameObject);    //�G���ł�邱�Ƃɂ���
            ////���G�łȂ����́A�_���[�W���󂯂�
            //else
            HP -= 1;
        }

        //�ϐg�̃t���O��ݒ�{�ϐg����
        if (other.gameObject == copyItem[0])
        {
            catFlag = true;
            duckFlag = false;
            penguinFlag = false;
            sheepFlag = false;
            Copy();
        }
        if (other.gameObject == copyItem[1])
        {
            catFlag = false;
            duckFlag = true;
            penguinFlag = false;
            sheepFlag = false;
            Copy();
        }
        if (other.gameObject == copyItem[2])
        {
            catFlag = false;
            duckFlag = false;
            penguinFlag = true;
            sheepFlag = false;
            Copy();
        }
        if (other.gameObject == copyItem[3])
        {
            catFlag = false;
            duckFlag = false;
            penguinFlag = false;
            sheepFlag = true;
            Copy();
        }
    }
}