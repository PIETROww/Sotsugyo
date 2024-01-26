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
    //public int ATK = 1;
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
    public enum Chara
    {
        Cat,
        Duck,
        Penguin,
        Sheep,
        Zako,
    }
    public Chara chara;
    public GameObject[] characters;
    bool catFlag = false,
        duckFlag = false,
        penguinFlag = false,
        sheepFlag = false,
        zakoFlag = false;

    //生成するアイテム
    public GameObject[] items;

    bool isDamaged = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        CharaSelect();
        //player.GetComponent<Player>().Copy(ref this.uniqueAction, ref this.characters, ref this.animator, ref this.attackObj,
        //    catFlag, duckFlag, penguinFlag, sheepFlag);
        Copy();
    }

    // Update is called once per frame
    void Update()
    {
        State_Think();
        State_Move();

        //確認用
        //Debug.Log(attackSensor);
    }

    void State_Think()
    {
        switch (state)
        {
            case State.Idle:
                if (sensor) { state = State.Capture; }
                if (isDamaged) { state = State.Damage; }
                if (HP <= 0) { state = State.Dead; }
                break;
            case State.Capture:
                if (!sensor) { state = State.Idle; }
                if (isDamaged) { state = State.Damage; }
                if (attackSensor && chara != Chara.Zako) { state = State.Attack; }
                if (HP <= 0) { state = State.Dead; }
                break;
            case State.Attack:
                if (!attackSensor)
                {
                    if (attackTime <= attackCnt)
                    {
                        attackCnt = 0.0f;
                        attackFlag = true;
                        if (chara == Chara.Cat || chara == Chara.Sheep)
                        {
                            attackObj.SetActive(false);
                        }
                        this.state = State.Idle;
                    }
                }
                if (isDamaged) { state = State.Damage; }
                if (HP <= 0) { state = State.Dead; }
                break;
            case State.Damage:
                isDamaged = false;
                state = State.Idle;
                if (HP <= 0) { state = State.Dead; }

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
                attackCnt += Time.deltaTime;
                if (attackFlag && 0.5f <= attackCnt)
                {
                    Attack();
                    attackFlag = false;
                }
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
        //Debug.Log("agent dest:" + agent.destination);
    }

    void Attack()
    {
        uniqueAction.Action(attackObj, animator, attackCnt);
    }


    void Damage()
    {
        //ヒットストップとかあったらbetter
        //後ろに跳ねる

        //HPが1減る
        HP -= 1;
    }
    void Dead()
    {
        //変身アイテムを生成
        switch (chara)
        {
            case Chara.Cat:
                Instantiate(items[0], transform.position, Quaternion.identity);
                break;
            case Chara.Duck:
                Instantiate(items[1], transform.position, Quaternion.identity);
                break;
            case Chara.Penguin:
                Instantiate(items[2], transform.position, Quaternion.identity);
                break;
            case Chara.Sheep:
                Instantiate(items[3], transform.position, Quaternion.identity);
                break;
        }

        Destroy(gameObject);

    }

    void CharaSelect()
    {
        switch (chara)
        {
            case Chara.Cat:
                catFlag = true;
                duckFlag = false;
                penguinFlag = false;
                sheepFlag = false;
                //player.GetComponent<Player>().Copy(ref this.uniqueAction, ref this.characters, ref this.animator, ref this.attackObj,
                //     catFlag, duckFlag, penguinFlag, sheepFlag);
                break;

            case Chara.Duck:
                catFlag = false;
                duckFlag = true;
                penguinFlag = false;
                sheepFlag = false;
                //player.GetComponent<Player>().Copy(ref this.uniqueAction, ref this.characters, ref this.animator, ref this.attackObj,
                //    catFlag, duckFlag, penguinFlag, sheepFlag);
                break;

            case Chara.Penguin:
                catFlag = false;
                duckFlag = false;
                penguinFlag = true;
                sheepFlag = false;
                //player.GetComponent<Player>().Copy(ref this.uniqueAction, ref this.characters, ref this.animator, ref this.attackObj,
                //    catFlag, duckFlag, penguinFlag, sheepFlag);
                break;

            case Chara.Sheep:
                catFlag = false;
                duckFlag = false;
                penguinFlag = false;
                sheepFlag = true;
                //player.GetComponent<Player>().Copy(ref this.uniqueAction, ref this.characters, ref this.animator, ref this.attackObj,
                //    catFlag, duckFlag, penguinFlag, sheepFlag);
                break;
            case Chara.Zako:
                catFlag = false;
                duckFlag = false;
                penguinFlag = false;
                sheepFlag = false;
                zakoFlag = true;
                break;
        }
    }
    public void Copy()
    {

        if (uniqueAction != null)
        {
            Destroy(uniqueAction);  //一度能力を消去
            uniqueAction = null;    //nullを入れる
        }

        if (catFlag)
        {
            characters[0].SetActive(true);
            characters[1].SetActive(false);
            characters[2].SetActive(false);
            characters[3].SetActive(false);
            characters[4].SetActive(false);
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
            characters[4].SetActive(false);
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
            characters[4].SetActive(false);
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
            characters[4].SetActive(false);
            animator = characters[3].GetComponent<Animator>();
            attackObj = sheepAttackObj;
            uniqueAction = gameObject.AddComponent<CharaUniqueActionSheep>();
        }
        if (zakoFlag)
        {
            characters[0].SetActive(false);
            characters[1].SetActive(false);
            characters[2].SetActive(false);
            characters[3].SetActive(false);
            characters[4].SetActive(true);
            animator = characters[4].GetComponent<Animator>();

            //animator = characters[4].GetComponent<Animator>();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == player)
        {
            //player.GetComponent<Player>().HP -= 1;    //プレイヤー側でやってる

            isDamaged = true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            isDamaged = true;
        }
    }
}