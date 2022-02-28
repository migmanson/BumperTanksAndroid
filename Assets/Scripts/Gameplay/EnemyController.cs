using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{
    public Transform[] controlpoints;
    public NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("ApplyDestination", 0, 4.0f);        
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
}
