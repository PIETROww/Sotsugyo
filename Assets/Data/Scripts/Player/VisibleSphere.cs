using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleSphere : MonoBehaviour
{
    public SphereCollider sphereCollider;
    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
    }
}
