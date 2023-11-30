using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
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
        if (sensor == false)
        {
            agent.destination = random.transform.position;
        }
        else
        {
            agent.destination = Target.transform.position;
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

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            
        }
    }
}
