using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
        Stop,
    }
    private State state;

    private CharaUniqueAction uniqueAction = null;

    private float inputX, inputZ;
    //InputSystem����
    private GameInputs gameInputs;
    private Vector2 moveInputValue;

    private Rigidbody rb;
    private Vector3 moveForward;

    [SerializeField] private Animator animator;
    private GameObject attackObj;
    public GameObject catAttackObj;
    public GameObject duckAttackObj;
    public GameObject penguinAttackObj;
    public GameObject sheepAttackObj;

    //�X�e�[�^�X--------------------------
    public int maxHP = 5;
    private int HP;
    public GameObject[] HPImage;
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

    ////�_���[�W���󂯂��Ƃ��̏���----------
    //public float damageTime = 2.0f;
    //private float damageCnt = 0.0f;
    //private bool isDamaged;
    //private MeshRenderer mesh;

    //���S--------------------------------
    public GameObject gameOverUI;

    //���G--------------------------------
    public float mutekiTime = 15.0f;
    private float timeCnt = 0.0f;
    private bool mutekiFlag = false;

    //�X�s�[�h�A�b�v----------------------
    public float speedUpTime = 15.0f;
    public float speedUpValue = 8.0f;
    private float defaultSpeed;

    //�G�t�F�N�g--------------------------
    //�����G�t�F�N�g
    public GameObject sunakemuriEffectPrefab;
    private float effectCnt = 0.0f;
    //���G�G�t�F�N�g
    public GameObject mutekiEffectPrefab;
    //�X�s�[�h�A�b�v�G�t�F�N�g
    public GameObject speedUpEffectPrefab;
    //�A�C�e���擾�G�t�F�N�g
    public GameObject getEffectPrefab;
    //�ϐg�G�t�F�N�g
    public GameObject changeEffectPrefab;
    private float stopCnt = 0.0f;
    private bool changeFlag = false;

    //�ϐg--------------------------------
    public enum Chara
    {
        Cat,
        Duck,
        Penguin,
        Sheep,
        Zako,
    }
    public Chara chara;

    public GameObject[] charactersLooks;
    //bool catFlag = false,
    //    duckFlag = false,
    //    penguinFlag = true,     //�ŏ��̓y���M���ɂ��Ă���
    //    sheepFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        //InputSystem�́AAction�N���X�̐ݒ�
        gameInputs = new GameInputs();
        //�eAction�C�x���g�o�^
        gameInputs.Player.Move.started += OnMove;       //started�F���͂���n��
        gameInputs.Player.Move.performed += OnMove;     //performed�F����̓��͂���������
        gameInputs.Player.Move.canceled += OnMove;      //canceled�F���͂𒆒f������
        gameInputs.Player.Jump.started += OnJump;
        gameInputs.Player.Jump.canceled += OffJump;
        //�L�������遨���̈�s���Ȃ��Ɠ��삵�Ȃ�
        gameInputs.Enable();

        HP = maxHP;
        rb = GetComponent<Rigidbody>();
        tr = this.transform;
        //mesh = GetComponent<MeshRenderer>();

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

        // �W�����v���̏���
        if (jumping)
        {
            if (jumpTime >= maxJumpTime)
            {
                jumping = false;
                jumpTime = 0;
            }
            else
            {
                jumpTime += Time.deltaTime;
            }
        }

        //State��Move��Ԃ̏ꍇ�A�����G�t�F�N�g���Đ�����
        if (this.state == State.Move)
        {
            sunakemuriEffect();
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

    //�ړ��{�^��--------------------------------
    private void OnMove(InputAction.CallbackContext context)
    {
        //Move�A�N�V�����̓��͎擾
        moveInputValue = context.ReadValue<Vector2>();
    }
    //�W�����v�{�^��-------------------------------------------------
    private void OnJump(InputAction.CallbackContext context)
    {
        //Jump�A�N�V�����̓��͎擾
        if (isGrounded)
        {
            jumping = true;
        }
    }
    private void OffJump(InputAction.CallbackContext context)
    {
        jumping = false;            //�~�����J�n
        jumpTime = 0;
    }
    private void StateThink()
    {
        switch (this.state)
        {
            case State.Idle:
                if (isGrounded && moveForward.magnitude > 0) { this.state = State.Move; }
                if (jumping) { this.state = State.Jump; }
                if (Input.GetKeyDown(KeyCode.N)) { this.state = State.Attack; }
                //if (isDamaged) { state = State.Damage; }
                if (HP <= 0) { this.state = State.Dead; }
                if (changeFlag) { this.state = State.Stop; }
                break;
            case State.Move:
                if (moveForward.magnitude <= 0) { this.state = State.Idle; }
                if (jumping) { this.state = State.Jump; }
                if (Input.GetKeyDown(KeyCode.N)) { this.state = State.Attack; }
                //if (isDamaged) { state = State.Damage; }
                if (HP <= 0) { this.state = State.Dead; }
                if (changeFlag) { this.state = State.Stop; }
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
                //if (isDamaged) { state = State.Damage; }
                if (HP <= 0) { this.state = State.Dead; }
                if (changeFlag) { this.state = State.Stop; }
                break;

            case State.Attack:
                if (attackTime <= attackCnt)
                {
                    attackCnt = 0.0f;
                    attackFlag = true;
                    if (chara != Chara.Penguin)
                    {
                        attackObj.SetActive(false);
                    }
                    this.state = State.Idle;
                }
                //if (isDamaged) { state = State.Damage; }
                if (HP <= 0) { this.state = State.Dead; }
                if (changeFlag) { this.state = State.Stop; }
                break;
            //case State.Damage:
            //    //if (damageTime <= damageCnt)
            //    //{
            //    //    damageCnt = 0.0f;
            //    //    this.state = State.Idle;
            //    //}
            //    isDamaged = false;
            //    state = State.Idle;
            //if (this.HP <= 0) { this.state = State.Dead; }
            //break;
            case State.Stop:
                if (1.0f <= stopCnt)
                {
                    stopCnt = 0.0f;
                    this.state = State.Idle;
                    changeFlag = false;
                }

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
            //case State.Damage:
            //    animator.SetTrigger("stun");
            //    //damageCnt += Time.deltaTime;
            //    Damage();
            //break;
            case State.Dead:
                Dead();
                break;
            case State.Stop:
                animator.SetTrigger("Idle");
                stopCnt += Time.deltaTime;
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
        HPImage[HP - 1].SetActive(false);
        HP -= 1;
    }

    void Dead()
    {
        gameOverUI.SetActive(true);
        //Time.timeScale = 0f;            //���Ԃ��~�߂�
    }

    private void Muteki()
    {
        //���G�t���O��true�̂Ƃ��Ɏ��s����
        if (mutekiFlag)
        {
            Debug.Log("���G���");

            //���G���Ԃ�i�߂�
            timeCnt += Time.deltaTime;
            //�_�ł�����
            //�G�t�F�N�g��t����ۂɃR�����g�A�E�g���܂���
            //if (timeCnt % 0.2f < 0.1f)
            //{
            //    mesh.enabled = true;
            //}
            //else
            //{
            //    mesh.enabled = false;
            //}

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
        defaultSpeed = speed;
        speed = speedUpValue;
        yield return new WaitForSeconds(speedUpTime);
        speed = defaultSpeed;
    }

    //�����G�t�F�N�g
    private void sunakemuriEffect()
    {
        effectCnt += Time.deltaTime;

        if (effectCnt >= 0.3)
        {
            GameObject sunakemuriEffect = Instantiate(sunakemuriEffectPrefab, new Vector3(transform.position.x, transform.position.y - 0.7f, transform.position.z), Quaternion.identity);
            Destroy(sunakemuriEffect, 1.0f);
            effectCnt = 0.0f;
        }
        if (this.state != State.Move)
        {
            effectCnt = 0.0f;
        }
    }

    //���G�G�t�F�N�g
    private void mutekiEffect()
    {
        GameObject mutekiEffect = Instantiate(mutekiEffectPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        mutekiEffect.transform.parent = this.gameObject.transform;
        Destroy(mutekiEffect, mutekiTime);
    }

    //�X�s�[�h�A�b�v�G�t�F�N�g
    private void speedUpEffect()
    {
        GameObject speedupEffect = Instantiate(speedUpEffectPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        speedupEffect.transform.parent = this.gameObject.transform;
        Destroy(speedupEffect, speedUpTime);
    }

    //�ϐg�G�t�F�N�g
    public void changeEffect()
    {
        GameObject changeEffect = Instantiate(changeEffectPrefab, this.transform.position, Quaternion.identity);
        Destroy(changeEffect, 1.0f);
    }

    //�A�C�e���擾�G�t�F�N�g
    private void itemGetEffect()
    {
        GameObject getEffect = Instantiate(getEffectPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Destroy(getEffect, 1.0f);
    }

    //void Falling()
    //{
    //    if (transform.position.y <= -10)
    //    {
    //        //�J�������Ƀ`�F�b�N�|�C���g�t�߂Ɉړ�������

    //        //�`�F�b�N�|�C���g�Ƀ��[�v����

    //        //���͂Ƃ肠�����Q�[���I�[�o�[��
    //        //HP = 0;
    //    }
    //}

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

        if (chara == Chara.Cat)
        {
            charactersLooks[0].SetActive(true);
            charactersLooks[1].SetActive(false);
            charactersLooks[2].SetActive(false);
            charactersLooks[3].SetActive(false);
            animator = charactersLooks[0].GetComponent<Animator>();
            attackObj = catAttackObj;
            uniqueAction = gameObject.AddComponent<CharaUniqueActionCat>();
        }
        if (chara == Chara.Duck)
        {
            charactersLooks[0].SetActive(false);
            charactersLooks[1].SetActive(true);
            charactersLooks[2].SetActive(false);
            charactersLooks[3].SetActive(false);
            animator = charactersLooks[1].GetComponent<Animator>();
            attackObj = duckAttackObj;
            uniqueAction = gameObject.AddComponent<CharaUniqueActionDuck>();
        }
        if (chara == Chara.Penguin)
        {
            charactersLooks[0].SetActive(false);
            charactersLooks[1].SetActive(false);
            charactersLooks[2].SetActive(true);
            charactersLooks[3].SetActive(false);
            animator = charactersLooks[2].GetComponent<Animator>();
            attackObj = penguinAttackObj;
            uniqueAction = gameObject.AddComponent<CharaUniqueActionPenguin>();
        }
        if (chara == Chara.Sheep)
        {
            charactersLooks[0].SetActive(false);
            charactersLooks[1].SetActive(false);
            charactersLooks[2].SetActive(false);
            charactersLooks[3].SetActive(true);
            animator = charactersLooks[3].GetComponent<Animator>();
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


    //public GameObject[] copyItem;
    private void OnCollisionEnter(Collision other)
    {
        //�Ԃ������Ώۂ����G�A�C�e���̏ꍇ
        if (other.gameObject.tag == "MutekiItem")
        {
            mutekiFlag = true;
            mutekiEffect();
            itemGetEffect();
        }

        //�Ԃ������Ώۂ��X�s�[�h�A�b�v�A�C�e���̏ꍇ
        if (other.gameObject.tag == "SpeedupItem")
        {
            StartCoroutine("SpeedUp");
            speedUpEffect();
            itemGetEffect();
        }

        //�Ԃ������Ώۂ��񕜃A�C�e���̏ꍇ
        if (other.gameObject.tag == "HealItem")
        {
            itemGetEffect();

            HP += 1;
            //HP��maxHP�ȏ�񕜂��Ȃ��悤�ɂ���
            if (HP > maxHP)
            {
                HP = maxHP;
            }
            HPImage[HP - 1].SetActive(true);
        }

        if (other.gameObject.tag == "Enemy")
        {
            ////���G�̎��́A�G��|��
            //if (mutekiFlag)
            //    Destroy(other.gameObject);    //�G���ł�邱�Ƃɂ���
            ////���G�łȂ����́A�_���[�W���󂯂�
            //else
            if (state != State.Dead)
            {
                if (!mutekiFlag)
                    Damage();
            }

            //isDamaged = true;
        }

        //�ϐg�̃t���O��ݒ�{�ϐg����
        if (other.gameObject.tag == "ChangeItem_Cat")
        {
            if (chara != Chara.Cat)
            {
                //changeFlag = true;
                //changeEffect();
                chara = Chara.Cat;
                Copy();
            }
        }
        if (other.gameObject.tag == "ChangeItem_Duck")
        {
            if (chara != Chara.Duck)
            {
                //changeFlag = true;
                //changeEffect();
                chara = Chara.Duck;
                Copy();
            }
        }
        if (other.gameObject.tag == "ChangeItem_Penguin")
        {
            if (chara != Chara.Penguin)
            {
                //changeFlag = true;
                //changeEffect();
                chara = Chara.Penguin;
                Copy();
            }
        }
        if (other.gameObject.tag == "ChangeItem_Sheep")
        {
            if (chara != Chara.Sheep)
            {
                //changeFlag = true;
                //changeEffect();
                chara = Chara.Sheep;
                Copy();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyAttack")
        {
            //isDamaged = true;
            if (state != State.Dead)
            {
                if (!mutekiFlag)
                    Damage();
            }
        }
    }
}