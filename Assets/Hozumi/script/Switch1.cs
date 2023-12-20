using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch1 : MonoBehaviour
{
    float bottomY = 1.0f;
    float speed = 1.0f;

    bool active;

    public Wall1 wall;

    public AudioClip switchSE;
    private AudioSource audioSource;

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

            if(transform.position.y<= bottomY)
            {
                audioSource.Play();
                wall.isOpen = true;
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
