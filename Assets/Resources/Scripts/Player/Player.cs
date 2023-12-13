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

    private PlayerUniqueAction action = null;

    private float inputX, inputZ;
    private Rigidbody rb;
    private Vector3 moveForward;

    [SerializeField] private Animator animator;
    private GameObject attackObj;
    public GameObject penguinAttackObj;
    public GameObject catAttackObj;
    public GameObject sheepAttackObj;

    //ステータス--------------------------
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

    //攻撃--------------------------------
    public GameObject attackRange;
    public float attackTime = 1.0f;
    private float attackCnt = 0.0f;

    //ダメージを受けたときの処理----------
    public float damageTime = 2.0f;
    private float damageCnt = 0.0f;

    //無敵--------------------------------
    public float mutekiTime = 10.0f;
    private float timeCnt = 0.0f;
    private bool mutekiFlag = false;

    //変身--------------------------------
    public GameObject[] characters;
    bool catFlag = false,
        duckFlag = false,
        penguinFlag = true,     //最初はペンギンにしておく
        sheepFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = this.transform;

        Copy();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");


        //ここからジャンプ
        isGrounded = CheckGroundStatus();

        // ジャンプの開始判定
        if (isGrounded && Input.GetButtonDown(JumpButtonName))
        {
            jumping = true;
        }
        // ジャンプ中の処理
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

        //確認
        //Debug.Log(moveForward);
        //Debug.Log(isGrounded);
        Debug.Log(this.state);

        //スフィアキャストの確認用
        //var start = transform.position + groundCheckOffsetY * Vector3.up;
        //var end = transform.position + groundCheckOffsetY * Vector3.up + groundCheckDistance * Vector3.down;
        //Debug.DrawLine(start, end, Color.red);
        //Debug.Log("start:" + start + " end:" + end);
    }


    private void FixedUpdate()
    {
        //カメラの向きを基準にした正面のベクトル
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        //moving = new Vector3(inputX, 0, inputZ);
        //moving = moving.normalized;

        //カメラ基準の動き
        moveForward = cameraForward * inputZ + Camera.main.transform.right * inputX;
        moveForward = moveForward.normalized;

        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }

        //状態遷移
        StateThink();
        //状態毎の処理
        StateMove();
        //無敵状態
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
                //ダメージへの遷移
                break;
            case State.Move:
                if (moveForward.magnitude <= 0) { this.state = State.Idle; }
                if (jumping) { this.state = State.Jump; }
                if (Input.GetKeyDown(KeyCode.N)) { this.state = State.Attack; }
                //ダメージへの遷移
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
                //ダメージへの遷移
                break;

            case State.Attack:
                if (attackTime <= attackCnt)
                {
                    attackCnt = 0.0f;
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
                Attack();
                break;
            case State.Damage:
                animator.SetTrigger("stun");
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
        // ジャンプの速度をアニメーションカーブから取得
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
        attackCnt += Time.deltaTime;
        action.Action(attackObj,animator,attackCnt);
    }

    void Damage()
    {
        damageCnt += Time.deltaTime;
    }

    void Dead()
    {

    }


    private void Muteki()
    {
        //無敵フラグがtrueのときに実行する
        if (mutekiFlag)
        {
            Debug.Log("無敵状態");

            //無敵時間を進める
            timeCnt += Time.deltaTime;

            //無敵時間を過ぎたとき
            if (timeCnt >= mutekiTime)
            {
                Debug.Log("無敵状態終わり");

                //無敵フラグをfalseにする
                mutekiFlag = false;
                //無敵時間をリセットする
                timeCnt = 0.0f;
            }
        }
    }

    private void Copy()
    {
        if(action != null)
        {
            Destroy(action);
            action = null;
        }

        if (catFlag)
        {
            characters[0].SetActive(true);
            characters[1].SetActive(false);
            characters[2].SetActive(false);
            characters[3].SetActive(false);
            animator = characters[0].GetComponent<Animator>();
            attackObj = catAttackObj;
            action = gameObject.AddComponent<PlayerUniqueActionCat>();
        }
        if (duckFlag)
        {
            characters[0].SetActive(false);
            characters[1].SetActive(true);
            characters[2].SetActive(false);
            characters[3].SetActive(false);
            animator = characters[1].GetComponent<Animator>();

            action = gameObject.AddComponent<PlayerUniqueActionDuck>();
        }
        if (penguinFlag)
        {
            characters[0].SetActive(false);
            characters[1].SetActive(false);
            characters[2].SetActive(true);
            characters[3].SetActive(false);
            animator = characters[2].GetComponent<Animator>();
            attackObj = penguinAttackObj;
            action = gameObject.AddComponent<PlayerUniqueActionPenguin>();
        }
        if (sheepFlag)
        {
            characters[0].SetActive(false);
            characters[1].SetActive(false);
            characters[2].SetActive(false);
            characters[3].SetActive(true);
            animator = characters[3].GetComponent<Animator>();
            attackObj= sheepAttackObj;
            action = gameObject.AddComponent<PlayerUniqueActionSheep>();
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
        //ぶつかった対象が無敵アイテムのタグの場合
        if (other.gameObject.tag == "MutekiItem")
            mutekiFlag = true;

        if (other.gameObject.tag == "Enemy")
        {
            //無敵の時は、敵を倒す
            if (mutekiFlag)
                Destroy(other.gameObject);
            //無敵でない時は、ダメージを受ける
            else
                HP -= 1;
        }

        if (other.gameObject == copyItem[0])
        {
            //変身
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