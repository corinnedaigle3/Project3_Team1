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
    public AudioSource seeSFX;
    bool sfxPlaying;
 
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

            if (!sfxPlaying && seeSFX != null)
            {
                seeSFX.Play();
                sfxPlaying = true;
            }
            agent.speed = 14;
            agent.acceleration = 20;
            playerLastPostion = player.transform.position; // save player postion
            // Chase Player
            agent.SetDestination(player.transform.position);
            //transform.LookAt(player.transform.position);

            Debug.Log("this is chased");

        }
       
    }
    private void Patrol()
    {
        sfxPlaying = false;
        if (gameObject.tag == "EnemyE" || gameObject.tag == "EnemyA" || gameObject.tag == "EnemyT")
        {
            agent.speed = 8f;
            agent.acceleration = 10f;
        } else
        {
            agent.speed = 5.5f;
            agent.acceleration = 8f;
        }

        // chose a new random waypoint when reach destination
        if (agent.remainingDistance <= 0.1f)
        {

            StartCoroutine(NewDesitnation(1.5f));
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
       // animator.SetBool("walking", false);// update animator


        yield return new WaitForSeconds(waitTime);
        if (Waypoints != null)
        {
            waypointIdnex = Random.Range(0, Waypoints.Length);
            lookingNew = false;
            isSearching = false;
            agent.SetDestination(Waypoints[waypointIdnex].position);
        }
        //animator.SetBool("walking", true); // update animator

    }


    public IEnumerator SearchArea()
    {
        isSearching= true;
        for (int i = 0; i < 3; i++)
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

    }
