using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyBehavior : MonoBehaviour
{

    [SerializeField] Transform[] Waypoints;
    public NavMeshAgent agent;
    public string enemyType;
    public bool playerLose = false;

    int waypointIdnex;

    // Start is called before the first frame update
    void Start()
    {
        enemyType = gameObject.name;
        waypointIdnex = Random.Range(0, Waypoints.Length);
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Waypoints[waypointIdnex].position);
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();

        if (playerLose) // add all the logic for ending the game    
        {
            StartCoroutine(LoseGame(1f));
            agent.isStopped = true;
        }
    }

    private void Patrol()
    {
        // chose a new random waypoint when reach destination
       if (agent.remainingDistance <= 0.1)
        {
            waypointIdnex = Random.Range(0, Waypoints.Length);

            agent.SetDestination(Waypoints[waypointIdnex].position);
        }
    }

    /*IEnumerator NewDesitnation (float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        waypointIdnex = Random.Range(0, Waypoints.Length);

        agent.SetDestination(Waypoints[waypointIdnex].position);
    }*/

    IEnumerator LoseGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("LOSE");

    }
}
