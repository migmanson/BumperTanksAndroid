using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{
    public Transform[] controlpoints;
    public NavMeshAgent agent;
    private Transform player;
    private Animator animatorcontroller;
    private short lives;

    void Start()
    {
        lives = 3;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("ApplyDestination", 0, 4.0f);
        animatorcontroller = GetComponentInChildren<Animator>();
    }

    public void ApplyDestination()
    {
        //agent.SetDestination(player.position);
        
        agent.SetDestination(controlpoints[Random.Range(0, controlpoints.Length)].position);
        //Debug.LogError(agent.destination);
    }

    private void Update()
    {
        //ApplyDestination();
        if (lives <= 0) 
        {
            //Debug.LogError("lives " + lives);
            animatorcontroller.SetBool("muere", true);
            Destroy(this.gameObject, 3);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // choques fuertes
        if (collision.relativeVelocity.magnitude > 10 && collision.collider.tag == "Player")
        {
            lives--;
            //Debug.LogError("lives " + lives);
            if (lives > 0)
            {
                animatorcontroller.SetBool("duele", true);
                Invoke("ResetAnimationHit", 0.2f);
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
        lives--;
        //Debug.LogError("lives " + lives);
        //Debug.LogError("en enemy controller  "  + other.name);
        if (lives > 0)
        {
            animatorcontroller.SetBool("duele", true);
            Invoke("ResetAnimationHit", 0.2f);
        }
    }

}
