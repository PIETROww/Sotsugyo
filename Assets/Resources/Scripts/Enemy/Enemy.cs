using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    enum State
    {
        Idle,
        Capture,
        Attack,
        Damage,
        Dead,
    }
    State state;

    public Transform Target;
    public Transform random;

    [SerializeField] private Animator animator;
    GameObject attackObj;
    public GameObject catAttackObj;
    public GameObject duckAttackObj;
    public GameObject penguinAttackObj;
    public GameObject sheepAttackObj;

    public int HP = 2;
    public int ATK = 1;
    //public float speed;

    GameObject player;

    NavMeshAgent agent;
    bool sensor;
    bool attackSensor;

    //攻撃
    public float attackTime = 1.0f;    //攻撃の所要時間　アニメーションによって変えるつもり
    float attackCnt = 0.0f;
    bool attackFlag = true;
    CharaUniqueAction uniqueAction;

    //キャラ分け--------------------------------
    public int charaNum = 0;        //キャラクターを決定する値
    public GameObject[] characters;
    bool catFlag = false,           //0
        duckFlag = false,           //1
        penguinFlag = false,        //2
        sheepFlag = false,          //3
        zakoFlag = false;           //4

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        CharaSelect();
        //player.GetComponent<Player>().Copy(ref this.uniqueAction, ref this.characters, ref this.animator, ref this.attackObj,
        //    catFlag, duckFlag, penguinFlag, sheepFlag);
    }

    // Update is called once per frame
    void Update()
    {
        //if (sensor == false)
        //{
        //    agent.destination = random.transform.position;
        //}
        //else
        //{
        //    agent.destination = Target.transform.position;
        //}

        State_Think();
        State_Move();

        //確認用
        Debug.Log(attackSensor);
    }

    void State_Think()
    {
        switch (state)
        {
            case State.Idle:
                if (sensor) { state = State.Capture; }
                break;
            case State.Capture:
                if (!sensor) { state = State.Idle; }
                if (attackSensor) { state = State.Attack; }
                break;
            case State.Attack:
                if (!attackSensor)
                {
                    if (attackTime <= attackCnt)
                    {
                        attackCnt = 0.0f;
                        attackFlag = true;
                        this.state = State.Idle;
                    }
                }

                break;
            case State.Damage:
                break;
            case State.Dead:
                break;
        }
    }

    void State_Move()
    {
        switch (state)
        {
            case State.Idle:
                animator.SetTrigger("Idle");
                Idle();
                break;
            case State.Capture:
                animator.SetTrigger("Walk");
                Capture();
                break;
            case State.Attack:
                if (attackFlag && 0.5f <= attackCnt)
                {
                    Attack();
                    attackFlag = false;
                }
                break;
                case State.Damage:
                animator.SetTrigger("stun");
                break;
                case State.Dead:
                break;
        }
    }

    //センサー系------------------------------
    public void tuiseki()
    {
        sensor = true;
    }
    public void haikai()
    {
        sensor = false;
    }
    public void OnAttack()
    {
        attackSensor = true;
    }
    public void OffAttack()
    {
        attackSensor = false;
    }
    //----------------------------------------

    void Idle()
    {
        agent.destination = random.transform.position;
    }

    void Capture()
    {
        agent.destination = Target.transform.position;
    }

    void Attack()
    {
        uniqueAction.Action(attackObj, animator, attackCnt);
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == player)
        {
            //player.GetComponent<Player>().HP -= 1;    //プレイヤー側でやってる

            this.HP -= 1;
        }
    }

    void Damage()
    {
        //ヒットストップとかあったらbetter
        //後ろに跳ねる
    }
    void Dead()
    {
        //変身アイテムを生成
    }

    void CharaSelect()
    {
        switch (charaNum)
        {
            case 0:
                catFlag = true;
                duckFlag = false;
                penguinFlag = false;
                sheepFlag = false;
                player.GetComponent<Player>().Copy(ref this.uniqueAction, ref this.characters, ref this.animator, ref this.attackObj,
                     catFlag, duckFlag, penguinFlag, sheepFlag);
                break;

            case 1:
                catFlag = false;
                duckFlag = true;
                penguinFlag = false;
                sheepFlag = false;
                player.GetComponent<Player>().Copy(ref this.uniqueAction, ref this.characters, ref this.animator, ref this.attackObj,
                    catFlag, duckFlag, penguinFlag, sheepFlag);
                break;

            case 2:
                catFlag = false;
                duckFlag = false;
                penguinFlag = true;
                sheepFlag = false;
                player.GetComponent<Player>().Copy(ref this.uniqueAction, ref this.characters, ref this.animator, ref this.attackObj,
                    catFlag, duckFlag, penguinFlag, sheepFlag);
                break;

            case 3:
                catFlag = false;
                duckFlag = false;
                penguinFlag = false;
                sheepFlag = true;
                player.GetComponent<Player>().Copy(ref this.uniqueAction, ref this.characters, ref this.animator, ref this.attackObj,
                    catFlag, duckFlag, penguinFlag, sheepFlag);
                break;
            case 4:
                catFlag = false;
                duckFlag = false;
                penguinFlag = false;
                sheepFlag = false;
                zakoFlag = true;
                break;
        }
    }

    //値を制限するための関数
    private void OnValidate()
    {
        charaNum = Mathf.Clamp(charaNum, 0, 4); //マジックナンバー
    }
}