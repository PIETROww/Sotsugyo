using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KibakoController : MonoBehaviour
{
    public GameObject healitemPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        Destroy(this.gameObject);
        Instantiate(healitemPrefab, transform.position, Quaternion.identity);
    }
}