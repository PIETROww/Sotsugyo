using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    float x, z;
    Rigidbody rb;
    Vector3 moving;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        moving = new Vector3(x, 0, z);

        moving = moving.normalized * speed;

        rb.velocity = moving;
    }
}
