using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyBehavior : MonoBehaviour
{

    [SerializeField] Transform[] Waypoints;
    public NavMeshAgent agent;
    public string enemyType;
    public bool playerLose = false;
    public GameManger gameManager;
    public UI ui;
    int waypointIdnex;
    public AudioSource caught;

    // Start is called before the first frame update
    void Start()
    {
        enemyType = gameObject.name;
        waypointIdnex = Random.Range(0, Waypoints.Length);
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Waypoints[waypointIdnex].position);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManger>();
        ui = GameObject.Find("Canvas").GetComponent<UI>();
    }

    // Update is called once per frame
    void Update()
    {
        //DieIfGemUsed();


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
        caught.Play();
        ui.LoadLose();

    }
    /*
    void DieIfGemUsed()
    {
        // if enemy is one of the 3 fury and their respective gem is used they will no longer spawn. 
        switch (enemyType)
        {
            case "FuryE":
                if (gameManager.furyE)
                {
                    Destroy(gameObject);
                }
                break;
            case "FuryA":
                if (gameManager.furyA)
                {
                    Destroy(gameObject);
                }
                break;
            case "FuryT":
                if (gameManager.furyT)
                {
                    Destroy(gameObject);
                }
                break;
            default:
                break;

        }
    }
    */
}
