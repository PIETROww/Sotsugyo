using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public GameObject enemy;
    Enemy encs;

    // Start is called before the first frame update
    void Start()
    {
        encs = enemy.GetComponent<Enemy>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            encs.tuiseki();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            encs.haikai();
        }
    }
}
