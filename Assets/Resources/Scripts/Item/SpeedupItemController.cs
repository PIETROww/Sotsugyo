using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedupItemController : MonoBehaviour
{
    public float pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x, pos + Mathf.PingPong(Time.time, 3.0f), this.transform.position.z);
        this.transform.Rotate(0.0f, 1.0f, 0.0f, Space.World);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}