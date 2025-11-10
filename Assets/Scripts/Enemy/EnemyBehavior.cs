using System.Collections;
using UnityEngine;
using UnityEngine.AI;


// Handles enemy AI behavior including patrol, chase, and search routines.
// Enemy will chase player when in line of sight, then search area if player escapes.
public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] bool shouldNotMove;
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
    bool playerInNvav; // True if the player is within the navigable area (NavMesh)
    bool isSearching;  // True while enemy is looking for the player’s last known position
    bool chasing;
    private Vector3 playerLastPostion;
    bool lookingNew;

    // Start is called before the first frame update
    void Start()
    {
        enemyType = gameObject.name;
        waypointIdnex = Random.Range(0, Waypoints.Length);
        agent = GetComponent<NavMeshAgent>();
        if (!shouldNotMove)
        {
            agent.SetDestination(Waypoints[waypointIdnex].position);
        }
        gameManager = GameObject.Find("GameManager").GetComponent<GameManger>();
      
        player = GameObject.Find("Player");
        lineOfSight = GetComponent<LineOfSight>();
        // let navmesh handle rotation 
        agent.updateRotation = true;
    }

    
    void FixedUpdate()
    {
        // Stop walking animation when the enemy reaches its destination
        if ((agent.remainingDistance < .2f || agent.isStopped == true) && animator != null && !shouldNotMove)
        {
            animator.SetBool("walking", false);
        }
        else if (animator != null)
        {
            animator.SetBool("walking", true);
        }

        // Ensure player reference is valid
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        // Check if the player is within the NavMesh to prevent invalid path errors
        playerInNvav = NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas);

        // Enemy logic:
        // - If player is visible and inside NavMesh, start chasing
        // - If player was being chased but is now lost, search last known area
        // - Otherwise, continue patrolling
        if (playerInNvav && lineOfSight.canChase && !shouldNotMove)
        {
            StopAllCoroutines(); // Stop any active search or patrol routines
            chasing = true;
            isSearching = false;
            chase();
        }
        else if (chasing && !shouldNotMove)
        {
            chasing = false;
            isSearching = true;
            StartCoroutine(SearchArea());
        }
        else if (!isSearching && !shouldNotMove)
        {
            Patrol();
        }
    }

    // Starts chasing the player using their current position.
    // Plays sound effect once when chase begins.
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
            

            Debug.Log("this is chased");

        }
       
    }
    private void Patrol()
    {
        // Set higher speed for smaller enemies 
        // (EnemyE, EnemyA, and EnemyT are faster units)
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
        
        }
    }

   // Chooses a new random waypoint for the enemy to patrol to after waiting.
    IEnumerator NewDesitnation (float waitTime)
    {
        if (lookingNew) // avid running again 
        {
            yield break;
        } else { lookingNew = true; }
       


        yield return new WaitForSeconds(waitTime);
        if (!shouldNotMove)
        {
            waypointIdnex = Random.Range(0, Waypoints.Length);
            lookingNew = false;
            isSearching = false;
            agent.SetDestination(Waypoints[waypointIdnex].position);
        }

    }

    // Searches around the player's last known position after losing sight.
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
