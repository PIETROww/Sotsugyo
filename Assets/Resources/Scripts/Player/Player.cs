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
    State state;

    float inputX, inputZ;
    Rigidbody rb;
    Vector3 moveForward;

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

    bool isGrounded = false;
    bool jumping = false;
    float jumpTime = 0;
    RaycastHit hit;
    Transform tr;
    //-----------------------------------


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
        //Debug.Log(isGrounded);

        // ジャンプの開始判定
        if (isGrounded && Input.GetButton(JumpButtonName))
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
            transform.rotation=Quaternion.LookRotation(moveForward);
        }

        StateThink();
        StateMove();
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
                if (this.HP <= 0) { this.state = State.Dead; }
                break;
            case State.Move:
                if (moveForward.magnitude <= 0) { this.state = State.Idle; }
                if (jumping) { this.state = State.Jump; }
                //攻撃への遷移
                //ダメージへの遷移
                //死亡への遷移
                break;
            case State.Jump:
                if (isGrounded) { this.state = State.Idle; }
                //攻撃への遷移
                //ダメージへの遷移
                //死亡への遷移
                break;

            case State.Attack:
                break;
            case State.Damage:
                break;

        }
    }

    void StateMove()
    {
        switch (this.state)
        {
            case State.Idle:

                break;
            case State.Move:
                Move();
                break;
            case State.Jump:
                Move();
                Jump();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Damage:
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

    }

    void Damage()
    {

    }

    void Dead()
    {

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
}
