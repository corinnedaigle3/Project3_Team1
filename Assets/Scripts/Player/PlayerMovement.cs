using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

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
    public bool helmUsed;

    [Header("Other")]
    public bool lose;
    public gemsChecker theGemChecker;
    private bool canDodge;
    public bool dashUnlocked;

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        moveSpeed = 20f;
        canDash = false;
        Invisible = false;
        helmUsed = false;
        asphodelCollectionItem = false;
        elysiumCollectionItem = false;
        tartarusCollectionItem = false;
        canDodge = false;
        dashUnlocked = false;

        rb.drag = groundDrag;

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
        inputSystem.Player.Dash.performed += DashPlayer;
        inputSystem.Player.Dash.canceled += DashPlayer;
        inputSystem.Player.Invisible.performed += Invisibility;
        inputSystem.Player.Dodge.performed += DodgeEnemy;
        inputSystem.Player.TakeDown.performed += TakeDownAction;

        state = State.Normal;
    }

    private void OnDisable()
    {
        inputSystem.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Normal:
                grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGrounded);

             //   MyInput();
                SpeedControl();
                

                if (invisibleTimer <= 0)
                {
                    canDash = false;
                    Invisible = false;
                    helmUsed = false;
                    moveSpeed = 10f;
                }

                //check if player is on ground
                if (grounded && dashUnlocked == true)
                {
                    rb.drag = groundDrag;
                    canDodge = true;
                }
                else
                {
                    rb.drag = 0;
                    canDodge = false;   
                }

                invisibleTimer -= Time.deltaTime;

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

                if (invisibleTimer <= 0)
                {
                    canDash = false;
                    Invisible = false;
                    helmUsed = false;
                    moveSpeed = 10f;
                }
                //check if player is on ground
                if (grounded)
                {
                    rb.drag = groundDrag;
                    canDodge = true;
                }
                else
                {
                    rb.drag = 0;
                    canDodge = false;
                }

                break;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                //read controller input for player
                Vector2 inputVector = inputSystem.Player.Move.ReadValue<Vector2>();

                // Convert it to camera-relative movement
                Vector3 camRelativeMove = orientation.forward * inputVector.y + orientation.right * inputVector.x;
                camRelativeMove.y = 0; // Ensure movement stays on the ground

                //saving the vector info
                moveDirection = camRelativeMove;

                rb.AddForce(camRelativeMove.normalized * moveSpeed * 10f, ForceMode.Force);
                transform.rotation = Quaternion.LookRotation(moveDirection);
                //MovePlayer();
                break;

            case State.Rolling:
                break;
        }
    }

    public void TakeDownAction(InputAction.CallbackContext context)
    {
        // makes the enemy stop and destroys the collider when pressing Q
        if (canKill == true && currentEnemy != null && context.performed)
        {
            Debug.Log("Q is pressed");
            if (takeDown != null) // Add a null check
            {
                takeDown.dead = true;
                
                Debug.Log("is it dead " + takeDown.dead);
            }
            else
            {
                Debug.LogWarning("TakeDown component not found on currentEnemy!");
            }
        }

        // this is enabling to consume the item.
        // it is tied to the same game keybinds as take down 
        if (theGemChecker.canPressQ)
        {
            theGemChecker.qPressed = true; // tje rest will happen in gemsCheker script 
        }
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

    private void DashPlayer(InputAction.CallbackContext context)
    {
        if (canDash == true)
        {
            if (context.performed)
            {
                moveSpeed = 16f;
            }

            if (context.canceled)
            {
                moveSpeed = 10f;
            }
        }
    }

    private void DodgeEnemy(InputAction.CallbackContext context) 
    {
        if (context.performed && canDodge == true && dashUnlocked == true)
        {
            rollDirection = moveDirection;
            rollSpeed = 16f;
            state = State.Rolling;
        }
    }

    private void Invisibility(InputAction.CallbackContext context)
    {
        if (context.performed && inventoryManager.hasHelm == true)
        {
            Invisible = true;
            canDash = true;
            invisibleTimer = 6f;
            helmUsed = true;
            inventoryManager.helmcounter -= 1;
            inventoryManager.ShowAmount(inventoryManager.helmText, inventoryManager.helmcounter, ref inventoryManager.hasHelm);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("colliding ");
        if (other.tag == "Behind" && inventoryManager.hasE == true)
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
            transform.LookAt(other.transform.position); // might remvoe it 
            gameObject.SetActive(false);
        }

        if (other.tag == "DashUnlocked")
        {
            canDodge = true;
            dashUnlocked = true;
        }

        switch (other.tag)
        {
            case "TakeDownItemE":

                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasE = true;

                    Debug.Log("Take down item picked up");
                    inventoryManager.takeDownItemCounterE++;
                    inventoryManager.takeDownItemTextE.gameObject.SetActive(true);
                    inventoryManager.ShowAmount(inventoryManager.takeDownItemTextE, inventoryManager.takeDownItemCounterE, ref inventoryManager.hasE);
                    Destroy(other.gameObject);
                    StartCoroutine(waitToFalse(0.5f));

                    Debug.Log("Has picked up item " + hasPickedUpItem);
                }

                break;

            case "TakeDownItemA":

                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasA = true;

                    inventoryManager.takeDownItemCounterA++;
                    inventoryManager.ShowAmount(inventoryManager.takeDownItemTextA, inventoryManager.takeDownItemCounterA, ref inventoryManager.hasA);
                    Destroy(other.gameObject);
                    StartCoroutine(waitToFalse(0.5f));


                }

                break;

            case "TakeDownItemT":

                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasT = true;

                    inventoryManager.takeDownItemCounterT++;
                    inventoryManager.ShowAmount(inventoryManager.takeDownItemTextT, inventoryManager.takeDownItemCounterT, ref inventoryManager.hasT);
                    Destroy(other.gameObject);
                    StartCoroutine(waitToFalse(0.5f));


                }

                break;

            case "GemE":

                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasGemE = true;

                    inventoryManager.gemCounterE++;
                    inventoryManager.ShowAmount(inventoryManager.gemTextE, inventoryManager.gemCounterE, ref inventoryManager.hasGemE);
                    Destroy(other.gameObject);
                    
                    StartCoroutine(waitToFalse(0.5f));

                }

                break;

            case "GemA":

                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasGemA = true;

                    inventoryManager.gemCounterA++;
                    inventoryManager.ShowAmount(inventoryManager.gemTextA, inventoryManager.gemCounterA, ref inventoryManager.hasGemA);
                    Destroy(other.gameObject);
                   
                    StartCoroutine(waitToFalse(0.5f));

                }

                break;

            case "GemT":

                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasGemT = true;

                    inventoryManager.gemCounterT++;
                    inventoryManager.ShowAmount(inventoryManager.gemTextT, inventoryManager.gemCounterT, ref inventoryManager.hasGemT);
                    Destroy(other.gameObject);
                   
                    StartCoroutine(waitToFalse(0.5f));

                }

                break;

            case "Helm":

                if (!hasPickedUpItem) // Check if the Helm was already picked up
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasHelm = true;

                    inventoryManager.helmcounter++;
                    inventoryManager.ShowAmount(inventoryManager.helmText, inventoryManager.helmcounter, ref inventoryManager.hasHelm);
                    Destroy(other.gameObject);
                   
                    StartCoroutine(waitToFalse(0.5f));
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
            takeDown.dead = false;
            currentEnemy = null;
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            hasPickedUpItem = false; // Reset flag when leaving
            Destroy(other.gameObject);
        }

        if(other.tag == "WaterE")
        {
            //teleport player back to beginning of level
        }

        if (other.tag == "DashUnlocked")
        {
            canDodge = false;
            dashUnlocked = false;
        }
    }


    IEnumerator waitToFalse(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hasPickedUpItem = false ;
    }
}