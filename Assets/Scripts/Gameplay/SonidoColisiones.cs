using UnityEngine;

public class SonidoColisiones : MonoBehaviour
{

    AudioSource audioSource;
    public AudioClip[] sonidos;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 6)
            audioSource.PlayOneShot(sonidos[0]);
        else if (collision.relativeVelocity.magnitude > 3)
            audioSource.PlayOneShot(sonidos[1]);
    }
}
