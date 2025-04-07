using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
   private enum State 
    {
        Normal,
        Rolling,
    }


    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    private Rigidbody rb;
    public static GameObject playerInstance;

    [Header("Input Actions")]
    private PlayerInput playerInput;
    private InputSystem inputSystem;

    [Header("BeatTheGame")]
    public int gemE; // just counter for how many fury gems we have 
    public int gemA; // just counter for how many fury gems we have 
    public int gemT; // just counter for how many fury gems we have 

    [Header("Movement")]
    public float moveSpeed;
    private float horizontalInput;
    private float verticalInput;
    Vector3 moveDirection; 
    private Vector3 rollDirection;
    private float rollSpeed;
    private State state;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGrounded;
    private bool grounded;
    public float groundDrag;

    [Header("Invisible")]
    private float invisibleTimer = 0;
    public bool Invisible;
    private bool canDash;

    [Header("Collection")]
    public bool asphodelCollectionItem;
    public bool elysiumCollectionItem;
    public bool tartarusCollectionItem;

    [Header("TakeDown")]
    EnemyBehavior enemyBehavior;
    [HideInInspector] public TakeDown takeDown;
    public bool dead;

    public bool fury1;
    public bool fury2;
    public bool fury3;

    [Header("Take down behavior")]
    public bool canKill;
    public GameObject currentEnemy;

    [Header("Inventory")]
    public InventoryManager inventoryManager;
    public bool helmInInventory;
    private bool triggerEnter;
    private bool hasPickedUpItem = false;

    [Header("Other")]
    public bool lose;

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        moveSpeed = 20f;
        canDash = false;
        Invisible = false;
        asphodelCollectionItem = false;
        elysiumCollectionItem = false;
        tartarusCollectionItem = false;

        fury1 = false;
        fury2 = false;
        fury3 = false;
    }

    private void Awake()
    {
        if (playerInstance != null && playerInstance != this.gameObject)
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
        else
        {
            playerInstance = this.gameObject;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }

        playerInput = GetComponent<PlayerInput>();

        inputSystem = new InputSystem();
        inputSystem.Player.Enable();

        state = State.Normal;
    }

    // Update is called once per frame
    void Update()
    {  
        switch (state)
        {
            case State.Normal:
                grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGrounded);

                MyInput();
                SpeedControl();
                DashPlayer();
                DodgeEnemy();
                Invisibility();

                //check if player is on ground
                if (grounded)
                {
                    rb.drag = groundDrag;
                }
                else
                {
                    rb.drag = 0;
                }

                invisibleTimer -= Time.deltaTime;

                // makes the enemy stop and destroys the collider when pressing Q
                if (canKill == true && currentEnemy != null && Input.GetKeyDown(KeyCode.Q))
                {
                    Debug.Log("Q is pressed");
                    if (takeDown != null) // Add a null check
                    {
                        takeDown.dead = true;
                        inventoryManager.TakeDownItemEcounter++;
                        inventoryManager.ShowAmount(inventoryManager.TakeDownItemEText, inventoryManager.TakeDownItemEcounter);
                        Debug.Log("is it dead " +  takeDown.dead);
                    }
                    else
                    {
                        Debug.LogWarning("TakeDown component not found on currentEnemy!");
                    }
                }

                break;

            case State.Rolling:
                rb.velocity = moveDirection * moveSpeed * rollSpeed;

                float rollSpeedDropMultiplier = 5f;
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;
                float rollSpeedMin = 15f;

                if (rollSpeed < rollSpeedMin)
                {
                    state = State.Normal;
                }

                invisibleTimer -= Time.deltaTime;
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                Vector2 inputVector = inputSystem.Player.Move.ReadValue<Vector2>();
                moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
                rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * moveSpeed * 10f, ForceMode.Force);
                transform.rotation = Quaternion.LookRotation(moveDirection);
                //MovePlayer();
                break;

            case State.Rolling:
                break;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void DashPlayer()
    {
        if (canDash == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                moveSpeed = 26f;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                moveSpeed = 20f;
            }
        }
    }

    private void DodgeEnemy() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rollDirection = moveDirection;
            rollSpeed = 16f;
            state = State.Rolling;
        }
    }

    private void Invisibility(/*InputAction.CallbackContext context*/)
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && inventoryManager.hasHelm == true)
        {
            Invisible = true;
            canDash = true;
            invisibleTimer = 6f;
            inventoryManager.helmcounter--;
            inventoryManager.ShowAmount(inventoryManager.helmText, inventoryManager.helmcounter);
        }

        if (invisibleTimer <= 0)
        {
            canDash = false;
            Invisible = false;
            moveSpeed = 20f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("colliding ");
        if (other.tag == "Behind")
        {
            inventoryManager.takeDowntext.SetActive(true);
            canKill = true;
            currentEnemy = other.gameObject;
            takeDown = currentEnemy.GetComponent<TakeDown>();
        }

        if (other.CompareTag("EnemyE") || other.CompareTag( "Fury") || other.CompareTag("EnemyA") || other.CompareTag("EnemyA"))
        {
            lose = true;
            other.gameObject.GetComponentInParent<EnemyBehavior>().playerLose = true;
            transform.LookAt(other.transform.position);
            gameObject.GetComponent<PlayerMovement>().enabled = false;
        }

        switch (other.tag)
        {
            case "TakeDownItemE":

                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasApple = true;
                    Debug.Log("tekdown should execute ");
                    inventoryManager.TakeDownItemEcounter++;
                    inventoryManager.ShowAmount(inventoryManager.TakeDownItemEText, inventoryManager.TakeDownItemEcounter);
                    Destroy(other.gameObject);
                }
               
                break;

            case "TakeDownItemA":

                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasSkull = true;

                    inventoryManager.TakeDownItemAcounter++;
                    inventoryManager.ShowAmount(inventoryManager.TakeDownItemAText, inventoryManager.TakeDownItemAcounter);
                    Destroy(other.gameObject);
                }
               
                break;

            case "TakeDownItemT":

                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasFireFlower = true;

                    inventoryManager.TakeDownItemTcounter++;
                    inventoryManager.ShowAmount(inventoryManager.TakeDownItemTText, inventoryManager.TakeDownItemTcounter);
                    Destroy(other.gameObject);
                }
               
                break;

            case "GemE":

                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasGemE = true;

                    inventoryManager.GemEcounter++;
                    inventoryManager.ShowAmount(inventoryManager.gemEText, inventoryManager.GemEcounter);
                    Destroy(other.gameObject);
                }
               
                break;

            case "GemA":

                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasGemA = true;

                    inventoryManager.GemAcounter++;
                    inventoryManager.ShowAmount(inventoryManager.gemAText, inventoryManager.GemAcounter);
                    Destroy(other.gameObject);
                }
              
                break;

            case "GemT":

                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasGemT = true;

                    inventoryManager.GemTcounter++;
                    inventoryManager.ShowAmount(inventoryManager.gemTText, inventoryManager.GemTcounter);
                    Destroy(other.gameObject);
                }
               
                break;

            case "Helm":

                if (!hasPickedUpItem) // Check if the Helm was already picked up
                {
                    hasPickedUpItem = true;
                    Debug.Log("Helm Is picked up");
                    inventoryManager.hasHelm = true;
                    inventoryManager.helmcounter++;
                    inventoryManager.ShowAmount(inventoryManager.helmText, inventoryManager.helmcounter);
                    Destroy(other.gameObject);
                }
                break;

             default:
                break;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Behind")
        {
            //take down enemy text
            inventoryManager.takeDowntext.SetActive(false);
            canKill = false;
            currentEnemy = null;
        }
       
            hasPickedUpItem = false; // Reset flag when leaving
    }
}