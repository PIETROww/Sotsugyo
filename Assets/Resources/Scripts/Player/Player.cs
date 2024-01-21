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
        Stop,
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

    //ステータス--------------------------
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

    //攻撃--------------------------------
    //public GameObject attackRange;
    public float attackTime = 1.0f;
    private float attackCnt = 0.0f;
    private bool attackFlag = true;

    //ダメージを受けたときの処理----------
    public float damageTime = 2.0f;
    private float damageCnt = 0.0f;
    private bool isDamaged;
    private MeshRenderer mesh;

    //無敵--------------------------------
    public float mutekiTime = 15.0f;
    private float timeCnt = 0.0f;
    private bool mutekiFlag = false;

    //スピードアップ----------------------
    public float speedUpTime = 15.0f;
    public float speedUpValue = 8.0f;
    private float defaultSpeed;

    //エフェクト--------------------------
    //砂煙エフェクト
    public GameObject sunakemuriEffectPrefab;
    private float effectCnt = 0.0f;
    //無敵エフェクト
    public GameObject mutekiEffectPrefab;
    //スピードアップエフェクト
    public GameObject speedUpEffectPrefab;
    //アイテム取得エフェクト
    public GameObject getEffectPrefab;
    //変身エフェクト
    public GameObject changeEffectPrefab;
    private float stopCnt = 0.0f;
    private bool changeFlag = false;

    //変身--------------------------------
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
    //    penguinFlag = true,     //最初はペンギンにしておく
    //    sheepFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        rb = GetComponent<Rigidbody>();
        tr = this.transform;
        mesh = GetComponent<MeshRenderer>();

        //Copy(ref uniqueAction, ref characters, ref animator, ref attackObj,
        //    catFlag, duckFlag, penguinFlag, sheepFlag);
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

        //StateがMove状態の場合、砂煙エフェクトを再生する
        if (this.state == State.Move)
        {
            sunakemuriEffect();
        }

        //確認
        //Debug.Log(moveForward);
        //Debug.Log(isGrounded);
        //Debug.Log(this.state);

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
                //if (isDamaged) { state = State.Damage; }
                if (changeFlag) { this.state = State.Stop; }
                break;
            case State.Move:
                if (moveForward.magnitude <= 0) { this.state = State.Idle; }
                if (jumping) { this.state = State.Jump; }
                if (Input.GetKeyDown(KeyCode.N)) { this.state = State.Attack; }
                //if (isDamaged) { state = State.Damage; }
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
                if (changeFlag) { this.state = State.Stop; }
                break;

            case State.Attack:
                if (attackTime <= attackCnt)
                {
                    attackCnt = 0.0f;
                    attackFlag = true;
                    this.state = State.Idle;
                }
                //if (isDamaged) { state = State.Damage; }
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
                Move();         //動きながら攻撃できるようにする
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
        uniqueAction.Action(attackObj, animator, attackCnt);
    }

    void Damage()
    {
        HPImage[HP - 1].SetActive(false);
        HP -= 1;
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
            //点滅させる
            //エフェクトを付ける際にコメントアウトしました
            //if (timeCnt % 0.2f < 0.1f)
            //{
            //    mesh.enabled = true;
            //}
            //else
            //{
            //    mesh.enabled = false;
            //}

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

    //スピードアップ
    IEnumerator SpeedUp()
    {
        defaultSpeed = speed;
        speed = speedUpValue;
        yield return new WaitForSeconds(speedUpTime);
        speed = defaultSpeed;
    }

    //砂煙エフェクト
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

    //無敵エフェクト
    private void mutekiEffect()
    {
        GameObject mutekiEffect = Instantiate(mutekiEffectPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        mutekiEffect.transform.parent = this.gameObject.transform;
        Destroy(mutekiEffect, mutekiTime);
    }

    //スピードアップエフェクト
    private void speedUpEffect()
    {
        GameObject speedupEffect = Instantiate(speedUpEffectPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        speedupEffect.transform.parent = this.gameObject.transform;
        Destroy(speedupEffect, speedUpTime);
    }

    //変身エフェクト
    public void changeEffect()
    {
        GameObject changeEffect = Instantiate(changeEffectPrefab, this.transform.position, Quaternion.identity);
        Destroy(changeEffect, 1.0f);
    }

    //アイテム取得エフェクト
    private void itemGetEffect()
    {
        GameObject getEffect = Instantiate(getEffectPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Destroy(getEffect, 1.0f);
    }

    void Falling()
    {
        if (transform.position.y <= -10)
        {
            //カメラを先にチェックポイント付近に移動させる

            //チェックポイントにワープする

            //今はとりあえずゲームオーバーに
            //HP = 0;
        }
    }

    //変身
    //public void Copy(ref CharaUniqueAction uniqueAction, ref GameObject[] characters, ref Animator animator, ref GameObject attackObj,
    //    bool catFlag, bool duckFlag, bool penguinFlag, bool sheepFlag)  //引数多すぎ
    public void Copy()
    {

        if (uniqueAction != null)
        {
            Destroy(uniqueAction);  //一度能力を消去
            uniqueAction = null;    //nullを入れる
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

    //着地処理を可視化するための処理
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
        //ぶつかった対象が無敵アイテムの場合
        if (other.gameObject.tag == "MutekiItem")
        {
            mutekiFlag = true;
            mutekiEffect();
            itemGetEffect();
        }

        //ぶつかった対象がスピードアップアイテムの場合
        if (other.gameObject.tag == "SpeedupItem")
        {
            StartCoroutine("SpeedUp");
            speedUpEffect();
            itemGetEffect();
        }

        //ぶつかった対象が回復アイテムの場合
        if (other.gameObject.tag == "HealItem")
        {
            itemGetEffect();

            HP += 1;
            //HPがmaxHP以上回復しないようにする
            if (HP > maxHP)
            {
                HP = maxHP;
            }
            HPImage[HP - 1].SetActive(true);
        }

        if (other.gameObject.tag == "Enemy")
        {
            ////無敵の時は、敵を倒す
            //if (mutekiFlag)
            //    Destroy(other.gameObject);    //敵側でやることにした
            ////無敵でない時は、ダメージを受ける
            //else
            if (state != State.Dead)
                Damage();
            //isDamaged = true;
        }

        //変身のフラグを設定＋変身する
        if (other.gameObject == copyItem[0])
        {
            changeFlag = true;
            changeEffect();
            chara = Chara.Cat;
            Copy();
        }
        if (other.gameObject == copyItem[1])
        {
            changeFlag = true;
            changeEffect();
            chara = Chara.Duck;
            Copy();
        }
        if (other.gameObject == copyItem[2])
        {
            changeFlag = true;
            changeEffect();
            chara = Chara.Penguin;
            Copy();
        }
        if (other.gameObject == copyItem[3])
        {
            changeFlag = true;
            changeEffect();
            chara = Chara.Sheep;
            Copy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyAttack")
        {
            //isDamaged = true;
            if (state != State.Dead)
                Damage();
        }
    }
}