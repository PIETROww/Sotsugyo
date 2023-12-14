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
    NavMeshAgent agent;
    bool sensor;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
                break;
            case State.Attack:
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
                Idle();
                break;
            case State.Capture:
                Capture();
                break;
        }
    }

    public void tuiseki()
    {
        sensor = true;
    }

    public void haikai()
    {
        sensor = false;
    }

    private void Idle()
    {
        agent.destination = random.transform.position;
    }

    private void Capture()
    {
        agent.destination = Target.transform.position;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {

        }
    }
}
