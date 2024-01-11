using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch2 : MonoBehaviour
{
    float bottomY = 25.0f;
    float speed = 1.0f;

    bool active;

    public Wall2 wall;

    public AudioClip switchSE;
    private AudioSource audioSource;

    [SerializeField] private ParticleSystem particle;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = switchSE;
    }
    void Update()
    {
        if (active && transform.position.y > bottomY)
        {
            transform.position -= Vector3.up * speed * Time.deltaTime;

            if (transform.position.y <= bottomY)
            {
                particle.Play();
                audioSource.Play();
                wall.isOpen1 = true;
                enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active && other.CompareTag("Player"))
        {
            active = true;
        }
    }
}
