using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCamera : MonoBehaviour
{
    private CinemachineVirtualCamera vCamera;
    private float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        vCamera = this.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        time+= Time.deltaTime;
        if(time>3)
        {
            Change();
        }
    }

    private void Change()
    {
        vCamera.Priority = 0;
    }
}
