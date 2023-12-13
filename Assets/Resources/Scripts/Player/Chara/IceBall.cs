using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    //éQè∆ÅFhttps://sleepygamersmemo.blogspot.com/2017/04/unity-raycast.html
    public float distanceFromSurface;
    public LayerMask targetLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity, targetLayer))
        {
            Vector3 newPos = transform.position;
            newPos.y = hitInfo.point.y + distanceFromSurface;
            transform.position = newPos;
        }
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward;
    }
}
