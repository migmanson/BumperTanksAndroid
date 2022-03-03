using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform[] controlpoints;
    public NavMeshAgent agent;
    private Transform player;
    private Animator animatorcontroller;
    public int health;
    public bool exitGarage = false;
    public MeshRenderer foco1;
    public MeshRenderer foco2;
    public MeshRenderer foco3;
    public Material matFocoVerde;
    public Material matFocoGris;
    public bool isDead;

    void Start()
    {
        health = 3;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 7;
        isDead = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.SetDestination(player.transform.position);
        animatorcontroller = GetComponentInChildren<Animator>();
        foco1.material = matFocoVerde;
        foco2.material = matFocoVerde;
        foco3.material = matFocoVerde;
    }

    public void ApplyDestination()
    {
        controlpoints = GameObject.Find("Controlpoints").GetComponentsInChildren<Transform>();
        agent.SetDestination(controlpoints[Random.Range(0, controlpoints.Length)].position);
    }


    void OnCollisionEnter(Collision collision)
    {
        // choques fuertes
        if (collision.relativeVelocity.magnitude > 10)
        {
            // Debug.LogError("lives " + lives + " collision: " + collision.collider.name);
            if (collision.relativeVelocity.magnitude > 20)
            {
                UpdateLives(3);
            }
            else if (collision.relativeVelocity.magnitude > 15)
            {
                UpdateLives(2);
            }
            else
            {
                UpdateLives(1);
            }

            //Debug.LogError("lives " + lives);
            if (health > 0)
            {
                animatorcontroller.SetBool("duele", true);
                SoundController.Instance.PlaySoundByIndex(5, this.transform.position);
                Invoke("ResetAnimationHit", 0.2f);
            }
            else
            {
                Dies();
            }
        }
        else if (collision.relativeVelocity.magnitude > 8 && collision.transform.tag == "Player")
        {
            // Sonido risa
            SoundController.Instance.PlaySoundByIndex(7, this.transform.position);
        }

    }

    void ResetAnimationHit()
    {
        animatorcontroller.SetBool("duele", false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            // colision con la bala
            UpdateLives(1);
            //Debug.LogError("lives " + lives + " collider: " + other.name);

            if (health > 0)
            {
                animatorcontroller.SetBool("duele", true);
                SoundController.Instance.PlaySoundByIndex(6, this.transform.position);
                Invoke("ResetAnimationHit", 0.2f);
            }
            else
            {
                Dies();
            }
        }
        else if (other.transform.name == "ExitGarageTrigger")
        {
            exitGarage = true;
            agent.speed = 4;
            InvokeRepeating("ApplyDestination", 0, 4.0f);
        }
    }

    void UpdateLives(int howManyLost = 1)
    {
        health = health - howManyLost;

        if (health == 2)
        {
            foco1.material = matFocoVerde;
            foco2.material = matFocoVerde;
            foco3.material = matFocoGris;
        }
        else if (health == 1)
        {
            foco1.material = matFocoVerde;
            foco2.material = matFocoGris;
            foco3.material = matFocoGris;
        }
        if (health <= 0)
        {
            foco1.material = matFocoGris;
            foco2.material = matFocoGris;
            foco3.material = matFocoGris;
        }
    }

    void Dies()
    {
        if (!isDead)
        {
            isDead = true;
            animatorcontroller.SetBool("muere", true);
            SoundController.Instance.PlaySoundByIndex(4, this.transform.position);
            GameController.Instance.EnemyDestroyed();
            Destroy(this.gameObject, 2.5f);            
        }
    }
}
