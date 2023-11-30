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

    private float inputX, inputZ;
    private Rigidbody rb;
    private Vector3 moveForward;

    [SerializeField] private Animator animator;

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

    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        Debug.Log(moveForward);

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
        //Debug.Log(isGrounded);
        Debug.Log(this.state);
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
        //変身
        Copy();
    }

    private void StateThink()
    {
        switch (this.state)
        {
            case State.Idle:
                if (isGrounded && moveForward.magnitude > 0) { this.state = State.Move; }
                if (jumping) { this.state = State.Jump; }
                //攻撃への遷移
                //ダメージへの遷移
                break;
            case State.Move:
                if (moveForward.magnitude <= 0) { this.state = State.Idle; }
                if (jumping) { this.state = State.Jump; }
                //攻撃への遷移
                //ダメージへの遷移
                break;
            case State.Jump:
                if (isGrounded) { this.state = State.Idle; }
                //攻撃への遷移
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
        attackRange.SetActive(true);
        attackCnt += Time.deltaTime;
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
        if (catFlag)
        {
            characters[0].SetActive(true);
            characters[1].SetActive(false);
            characters[2].SetActive(false);
            characters[3].SetActive(false);
            animator = characters[0].GetComponent<Animator>();
        }
        if (duckFlag)
        {
            characters[0].SetActive(false);
            characters[1].SetActive(true);
            characters[2].SetActive(false);
            characters[3].SetActive(false);
            animator = characters[1].GetComponent<Animator>();
        }
        if (penguinFlag)
        {
            characters[0].SetActive(false);
            characters[1].SetActive(false);
            characters[2].SetActive(true);
            characters[3].SetActive(false);
            animator = characters[2].GetComponent<Animator>();
        }
        if (sheepFlag)
        {
            characters[0].SetActive(false);
            characters[1].SetActive(false);
            characters[2].SetActive(false);
            characters[3].SetActive(true);
            animator = characters[3].GetComponent<Animator>();
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
            catFlag = true;
            duckFlag = false;
            penguinFlag = false;
            sheepFlag = false;
        }    
        if(other.gameObject == copyItem[1])
        {
            catFlag = false;
            duckFlag = true;
            penguinFlag = false;
            sheepFlag = false;
        }
        if (other.gameObject == copyItem[2])
        {
            catFlag = false;
            duckFlag = false;
            penguinFlag = true;
            sheepFlag = false;
        }
        if (other.gameObject == copyItem[3])
        {
            catFlag = false;
            duckFlag = false;
            penguinFlag = false;
            sheepFlag = true;
        }
    }
}
