using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{
    public Transform[] controlpoints;
    public NavMeshAgent agent;
    private Transform player;
    private Animator animatorcontroller;

    void Start()
    {
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
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 10)
        {
            animatorcontroller.SetBool("duele", true);
            Invoke("ResetAnimationHit", 0.2f);
        }
            
    }

    void ResetAnimationHit() {
        animatorcontroller.SetBool("duele", false);
    }

}
