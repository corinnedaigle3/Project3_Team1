using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{

    [SerializeField] Transform[] Waypoints;
    NavMeshAgent agent;

    int waypointIdnex;

    // Start is called before the first frame update
    void Start()
    {
        waypointIdnex = Random.Range(0, Waypoints.Length);
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Waypoints[waypointIdnex].position);
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        // chose a new random waypoint when reach destination
       if (agent.remainingDistance < 0.5)
        {
            Debug.Log("moving");
            waypointIdnex = Random.Range(0, Waypoints.Length);

            agent.SetDestination(Waypoints[waypointIdnex].position);
        }
    }
}
