using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class EnemyBehavior : MonoBehaviour
{

    [SerializeField] Transform[] Waypoints;
    public NavMeshAgent agent;
    public string enemyType;
    public bool playerLose = false;
    public GameManger gameManager;
    public UI ui;
    int waypointIdnex;
    public GameObject player;

    public LineOfSight lineOfSight;
    bool playerInNvav;
    bool isSearching;
    bool chasing;

    // Start is called before the first frame update
    void Start()
    {
        enemyType = gameObject.name;
        waypointIdnex = Random.Range(0, Waypoints.Length);
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Waypoints[waypointIdnex].position);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManger>();
        ui = GameObject.Find("Canvas").GetComponent<UI>();
        player = GameObject.Find("Player");
        lineOfSight = GetComponent<LineOfSight>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //DieIfGemUsed();
        NavMeshHit hit;
        playerInNvav = NavMesh.SamplePosition(player.transform.position, out hit, 1f, NavMesh.AllAreas);

        Debug.Log("Player in in navMesh" + playerInNvav);
        if (playerInNvav && lineOfSight.canChase)
        {
            chasing = true;
            isSearching = false;
            StopAllCoroutines();
            chase();
        } else
        {
            Patrol();

        }



        if (playerLose) // add all the logic for ending the game    
        {
            StartCoroutine(LoseGame(1f));

            agent.isStopped = true;
        }
    }
    void chase()
    {
        if (player != null && lineOfSight.canChase)
        {
            //gameMusic.GetComponent<MusicControlelr>().PChaseMusic();
            //pSprite.SetBool("isChased", true);
            // Chase Player
            agent.SetDestination(player.transform.position);
            transform.LookAt(player.transform.position);

            Debug.Log("this is chased");
            // increase rotation speed 
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

            isSearching = false;
            chasing = false;
        } 
        if (isSearching)
        {
            StartCoroutine(SearchArea());
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
        ui.LoadLose();

    }
    public IEnumerator SearchArea()
    {
        isSearching= true;
        for (int i = 0; i < 3; i++)
        {// look around random locations after chasing player
            Vector3 randomSearchPos = player.transform.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
            agent.SetDestination(randomSearchPos);
            transform.LookAt(randomSearchPos);

            Debug.Log("Searching area attempt: " + i);

          
            yield return new WaitForSeconds(2f);
        }
        isSearching = false ;
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
