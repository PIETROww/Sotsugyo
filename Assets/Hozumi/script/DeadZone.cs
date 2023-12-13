using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    Vector3 StartPos;
    // Start is called before the first frame update
    void Start()
    {
        StartPos= transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag=="Player")
        {
           // Player.transform = StartPos;
        }
    }
}
