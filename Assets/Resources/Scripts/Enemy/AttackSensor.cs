using UnityEngine;

public class AttackSensor : MonoBehaviour
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
            encs.OnAttack();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            encs.OffAttack();
        }
    }
}
