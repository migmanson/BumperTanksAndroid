using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{
    public Transform[] controlpoints;
    public NavMeshAgent agent;
    private Transform player;
    private Animator animatorcontroller;
    private int lives;
    public MeshRenderer foco1;
    public MeshRenderer foco2;
    public MeshRenderer foco3;
    public Material matFocoVerde;
    public Material matFocoGris;


    void Start()
    {
        lives = 3;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("ApplyDestination", 0, 4.0f);
        animatorcontroller = GetComponentInChildren<Animator>();
        foco1.material = matFocoVerde;
        foco2.material = matFocoVerde;
        foco3.material = matFocoVerde;
    }

    public void ApplyDestination()
    {
        //agent.SetDestination(player.position);
        
        agent.SetDestination(controlpoints[Random.Range(0, controlpoints.Length)].position);
        //Debug.LogError(agent.destination);
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
            else { 
                UpdateLives(1); 
            }

            //Debug.LogError("lives " + lives);
            if (lives > 0)
            {
                animatorcontroller.SetBool("duele", true);
                Invoke("ResetAnimationHit", 0.2f);
            }
            else {
                animatorcontroller.SetBool("muere", true);
                Destroy(this.gameObject, 3);
            }
        }
    }

    void ResetAnimationHit()
    {
        animatorcontroller.SetBool("duele", false);
    }

    void OnTriggerEnter(Collider other)
    {
        // colision con la bala
        UpdateLives(1);
        //Debug.LogError("lives " + lives + " collider: " + other.name);

        if (lives > 0)
        {
            animatorcontroller.SetBool("duele", true);
            Invoke("ResetAnimationHit", 0.2f);
        }
        else
        {
            animatorcontroller.SetBool("muere", true);
            Destroy(this.gameObject, 3);
        }
    }

    void UpdateLives(int howManyLost = 1)
    {
        lives = lives - howManyLost;

        if (lives == 2)
        {
            foco1.material = matFocoVerde;
            foco2.material = matFocoVerde;
            foco3.material = matFocoGris;
        }
        else if (lives == 1)
        {
            foco1.material = matFocoVerde;
            foco2.material = matFocoGris;
            foco3.material = matFocoGris;
        }
        if (lives <= 0)
        {
            foco1.material = matFocoGris;
            foco2.material = matFocoGris;
            foco3.material = matFocoGris;
        }
    }

}
