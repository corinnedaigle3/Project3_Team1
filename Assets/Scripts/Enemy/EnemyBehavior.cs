using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class EnemyBehavior : MonoBehaviour
{

    [SerializeField] Transform[] Waypoints;
    public NavMeshAgent agent;
    public string enemyType;
    public Animator animator;
 
    public GameManger gameManager;

    int waypointIdnex;
    public GameObject player;

    public LineOfSight lineOfSight;
    bool playerInNvav;
    bool isSearching;
    bool chasing;
    private Vector3 playerLastPostion;
    bool lookingNew;

    // Start is called before the first frame update
    void Start()
    {
        enemyType = gameObject.name;
        waypointIdnex = Random.Range(0, Waypoints.Length);
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Waypoints[waypointIdnex].position);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManger>();
      
        player = GameObject.Find("Player");
        lineOfSight = GetComponent<LineOfSight>();
        // let navmesh handle rotation 
        agent.updateRotation = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(agent.remainingDistance < .2f && animator != null)
        {
            animator.SetBool("walking", false);// update animator

        }
        else if ( animator != null)
        {
            animator.SetBool("walking", true);// update animator
        }

        //DieIfGemUsed();
        NavMeshHit hit;
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        playerInNvav = NavMesh.SamplePosition(player.transform.position, out hit, 1f, NavMesh.AllAreas);

        Debug.Log("Player in in navMesh" + playerInNvav);
        if (playerInNvav && lineOfSight.canChase)
        {
            StopAllCoroutines();
            chasing = true;
            isSearching = false;
            chase();
        }
        else if (chasing)
        {
            chasing = false;
            isSearching = true;
            StartCoroutine(SearchArea());
        }
        else if (!isSearching)
        {
            Patrol();
        }



    }
    void chase()
    {
        if (player != null && lineOfSight.canChase)
        {



            playerLastPostion = player.transform.position; // save player postion
            // Chase Player
            agent.SetDestination(player.transform.position);
            //transform.LookAt(player.transform.position);

            Debug.Log("this is chased");
            // increase rotation speed 
           /* 
            * Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            */

        }
       
    }
    private void Patrol()
    {
        // chose a new random waypoint when reach destination
        if (agent.remainingDistance <= 0.1f)
        {

            StartCoroutine(NewDesitnation(2f));
           // waypointIdnex = Random.Range(0, Waypoints.Length);

           // agent.SetDestination(Waypoints[waypointIdnex].position);
        }
    }

    IEnumerator NewDesitnation (float waitTime)
    {
        if (lookingNew)
        {
            yield break;
        } else { lookingNew = true; }
        //agent.isStopped = true;
        animator.SetBool("walking", false);// update animator


        yield return new WaitForSeconds(waitTime);
        waypointIdnex = Random.Range(0, Waypoints.Length);
        lookingNew = false;
        isSearching = false;
        agent.SetDestination(Waypoints[waypointIdnex].position);
        animator.SetBool("walking", true); // update animator

    }


    public IEnumerator SearchArea()
    {
        isSearching= true;
        for (int i = 0; i < 2; i++)
        {// look around random locations after chasing player
            Vector3 randomSearchPos = playerLastPostion + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomSearchPos, out hit, 2f, NavMesh.AllAreas))
            {

                agent.SetDestination(randomSearchPos);
                //transform.LookAt(randomSearchPos);

                Debug.Log("Searching area attempt: " + i);
            }
            else
            {
                continue;
            }
          

          
            yield return new WaitForSeconds(1f);
        }
        isSearching = false ;
        Patrol();
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
