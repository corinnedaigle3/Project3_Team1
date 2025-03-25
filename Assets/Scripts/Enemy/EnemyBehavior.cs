using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{

    [SerializeField] Transform[] Waypoints;
    public NavMeshAgent agent;

    int waypointIdnex;

    [SerializeField] GameObject bullet;
    [SerializeField] GameObject spawnPt;
    private int waitTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        waypointIdnex = Random.Range(0, Waypoints.Length);
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Waypoints[waypointIdnex].position);

        StartCoroutine(TimeShoot());
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

    void Shoot()
    {
        Instantiate(bullet, spawnPt.transform.position, spawnPt.transform.rotation);
        StartCoroutine(TimeShoot());
    }

    IEnumerator TimeShoot()
    {
        yield return new WaitForSeconds(waitTime);
        Shoot();
    }
}
