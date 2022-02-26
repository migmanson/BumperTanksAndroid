using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoColisiones : MonoBehaviour
{

    AudioSource audioSource;
    public AudioClip[] sonidos;
    public float fuerzaCantidad = 5;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 6)
            audioSource.PlayOneShot(sonidos[0]);
        else if (collision.relativeVelocity.magnitude > 3)
            audioSource.PlayOneShot(sonidos[1]);
    }
}
