using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class KibakoController : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject effectPrefab;
    //[SerializeField] private ParticleSystem particle;

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
       //ParticleSystem newParticle = Instantiate(particle);
        //newParticle.transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        //newParticle.Play();
        if (other.gameObject.tag == "Player")
        {
            GameObject destroyEffect = Instantiate(effectPrefab, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity);
            Destroy(destroyEffect, 3.0f);
            Destroy(this.gameObject);
            Instantiate(itemPrefab, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), itemPrefab.transform.rotation);
        }
    }
}
