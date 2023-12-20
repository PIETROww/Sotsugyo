using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    Vector3 StartPos;

    // Start is called before the first frame update
    void Start()
    {
        GameObject targetObject = GameObject.Find("StartPos");
        Transform targetTransform = targetObject.transform;
        Vector3 targetPosition = targetTransform.position;
        StartPos = targetPosition;
    }

   
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           other.transform.position = StartPos;
        }
    }
}
