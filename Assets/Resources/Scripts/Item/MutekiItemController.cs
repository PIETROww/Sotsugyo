using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MutekiItemController : MonoBehaviour
{
    public float pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform.position.y;
    }

    // Update is called once per frame
    private void Update()
    {
        this.transform.position = new Vector3(transform.position.x, pos + Mathf.PingPong(Time.time, 2.0f), transform.position.z);
        this.transform.Rotate(0.0f, 1.0f, 0.0f, Space.World);
    }

    //‚Ô‚Â‚©‚Á‚½‘ÎÛ‚ªPlayer‚¾‚Á‚½‚ç–³“GƒAƒCƒeƒ€‚ğÁ–Å‚³‚¹‚é
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
