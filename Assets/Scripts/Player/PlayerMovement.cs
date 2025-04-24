using System.Collections;
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
    //public static GameObject playerInstance;
    public UI ui;
    public GameObject vfx;

    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    private Rigidbody rb;

    [Header("Input Actions")]
    private PlayerInput playerInput;
    private InputSystem inputSystem;

    [Header("BeatTheGame")]
    public int gemE; // just counter for how many fury gems we have 
    public int gemA; // just counter for how many fury gems we have 
    public int gemT; // just counter for how many fury gems we have 

    [Header("Movement")]
    public float moveSpeed;
    public Vector3 moveDirection; 
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
    private bool canRun;

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

    [Header("Music")] 
    public AudioSource takeDownSound;
    public AudioSource pickupSound;
    public AudioSource caughtSound;
    public AudioSource dodgeSound;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    public float minSlopeAngle;
    public RaycastHit slopeHit;
    private float angle;

    [Header("Dodge")]
    public bool canDodge;
    public bool unlockDodge;
    public bool inDodgeRange;
    public float dodgeTimer;
    public bool isDodging;
    public float dodgeTimerFade;

    [Header("Other")]
    public bool lose;
    public gemsChecker theGemChecker;

    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.Find("Canvas").GetComponent<UI>();
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        moveSpeed = 8f;
        canRun = false;
        Invisible = false;
        helmUsed = false;
        asphodelCollectionItem = false;
        elysiumCollectionItem = false;
        tartarusCollectionItem = false;
        canDodge = false;
        unlockDodge = false;
        maxSlopeAngle = 50f;
        minSlopeAngle = 20f;
        isDodging = false;

        rb.drag = groundDrag;
        dodgeTimer = 0;
        inDodgeRange = false;

        fury1 = false;
        fury2 = false;
        fury3 = false;
    }

    private void Awake()
    {
/*
        if (playerInstance != null && playerInstance != this.gameObject)
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
        else
        {
            playerInstance = this.gameObject;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }

        */
        playerInput = GetComponent<PlayerInput>();

        inputSystem = new InputSystem();
        inputSystem.Player.Enable();
        inputSystem.Player.Dash.performed += PlayerRun; // this means run 
        inputSystem.Player.Dash.canceled += PlayerRun; // this means run
        inputSystem.Player.Invisible.performed += Invisibility;
        inputSystem.Player.Dodge.performed += DodgeEnemy;
        inputSystem.Player.TakeDown.performed += TakeDownAction;

        state = State.Normal;
    }

    private void OnDisable()
    {
        inputSystem.Player.Disable();
    }
    private void Update()
    {
        OnSlopeNow();
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                //read controller input for player
                Vector2 inputVector = inputSystem.Player.Move.ReadValue<Vector2>().normalized; 

                // Convert it to camera-relative movement
                Vector3 camRelativeMove = orientation.forward * inputVector.y + orientation.right * inputVector.x;
                camRelativeMove.y = 0; // Ensure movement stays on the ground

                //saving the vector info
                moveDirection = camRelativeMove;

                Vector3 moveDirToUse = moveDirection;

                if (OnSlope())
                {
                    moveDirToUse = GetSlopeMoveDirection();
                    rb.drag = groundDrag;
                }

                rb.MovePosition(rb.position + moveDirToUse * moveSpeed * Time.fixedDeltaTime);
                //rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

                if (moveDirection != Vector3.zero)
                {
                    // THIS IS HOW I FIXED THE ROTATION ISSUES (The rotation was instantanious and was causing the issue) 
                    Quaternion toRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
                }

                grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGrounded);

                //   MyInput();
                SpeedControl();
                StickToGround();

                if (invisibleTimer <= 0)
                {
                    ui.popUpBar2.SetActive(false);
                    canRun = false;
                    Invisible = false;
                    helmUsed = false;
                    moveSpeed = 8f; 
                    vfx.SetActive(false);
                    // inventoryManager.invisText.SetActive(false);
                }

                //Dodge icon fade logic timer
                if (dodgeTimerFade <= 0)
                {
                    isDodging = true;
                }
                else
                {
                    isDodging = false;
                }

                //Time allowed between dodge
                if (dodgeTimer <= 0)
                {
                    canDodge = true;
                }
                else
                {
                    canDodge = false;
                }

                //check if player is on ground
                if (grounded)
                {
                    rb.drag = groundDrag;
                    unlockDodge = true;
                }
                else
                {
                    unlockDodge = false;
                }

                invisibleTimer -= Time.deltaTime;
                dodgeTimer -= Time.deltaTime;
                dodgeTimerFade -= Time.deltaTime;
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

                if (invisibleTimer <= 0)
                {
                    ui.popUpBar2.SetActive(false);
                    canRun = false;
                    Invisible = false;
                    vfx.SetActive(false);
                    helmUsed = false;
                    moveSpeed = 8f;
                //    inventoryManager.invisText.SetActive(false);
                }
             

                //Dodge icon fade logic timer
                if (dodgeTimerFade <= 0)
                {
                    isDodging = true;
                }
                else
                {
                    isDodging = false;
                }

                //Time allowed between dodge
                if (dodgeTimer <= 0)
                {
                    canDodge = true;
                }
                else
                {
                    canDodge = false;
                }

                //check if player is on ground
                if (grounded)
                {
                    rb.drag = groundDrag;
                    unlockDodge = true;
                }
                else
                {
                    rb.drag = 0;
                    unlockDodge = false;
                }

                invisibleTimer -= Time.deltaTime;
                dodgeTimer -= Time.deltaTime;
                dodgeTimerFade -= Time.deltaTime;
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
                takeDownSound.Play();

                Debug.Log("is it dead " + takeDown.dead);
            }
            else
            {
                Debug.LogWarning("TakeDown component not found on currentEnemy!");
            }
        }

        // this is enabling to consume the item.
        // it is tied to the same game keybinds as take down 
        if (theGemChecker != null && theGemChecker.canPressQ)
        {
            theGemChecker.qPressed = true; // the rest will happen in gemsCheker script 
        }
    }

    private void SpeedControl()
    {
        //limiting speed on slope
        if (OnSlope())
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        //limiting speed on ground
        else
        { 
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void PlayerRun(InputAction.CallbackContext context)
    {
        if (canRun == true)
        {
            if (context.performed)
            {
                moveSpeed = 12f;
            }

            if (context.canceled)
            {
                moveSpeed = 8f;
            }
        }
    }

    private void DodgeEnemy(InputAction.CallbackContext context) 
    {
        if (context.performed && canDodge == true && unlockDodge == true && inDodgeRange == true)
        {
            rollDirection = moveDirection;
            rollSpeed = 17f;
            state = State.Rolling;
            dodgeTimer = 5f;
            dodgeSound.Play();
            dodgeTimerFade = 4.8f;
        }
    }

    private void Invisibility(InputAction.CallbackContext context)
    {
        if (context.performed && inventoryManager.hasHelm == true)
        {
            //ui.popUpBar2.SetActive(true);
            Invisible = true;
            canRun = true;
            vfx.SetActive(true);
          //  inventoryManager.helmUseText.SetActive(false);
            //inventoryManager.invisText.SetActive(true);
            invisibleTimer = 5f;
            helmUsed = true;
            inventoryManager.helmcounter -= 1;
            inventoryManager.ShowAmount(inventoryManager.helmText, inventoryManager.helmcounter, ref inventoryManager.hasHelm);
        }
    }

    public bool OnSlope()
    {
        float sphereCastRadius = 0.3f; // Match this to your CapsuleCollider radius
        float rayLength = playerHeight * 0.5f + 0.5f; // Add a little buffer
        Vector3 origin = transform.position;

        if (Physics.SphereCast(origin, sphereCastRadius, Vector3.down, out slopeHit, rayLength))
        {
            angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle > minSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void OnSlopeNow()
    {
        if (OnSlope())
        {
            Vector3 slopeMove = GetSlopeMoveDirection() * moveSpeed;
            rb.AddForce(slopeMove, ForceMode.Acceleration);

            // Small downward force to stick to slope
            if (rb.velocity.y <= 0.1f)
            {
                rb.AddForce(-slopeHit.normal * 100f, ForceMode.Force);
            }
        }
    }

    void StickToGround()
    {
        if (!OnSlope())
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f))
            {
                if (hit.distance > 0.05f)
                {
                    rb.AddForce(Vector3.down * 10f, ForceMode.Force);
                }
            }
        }
    }

    IEnumerator LoseGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ui.LoadLose();

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("colliding");
        /*
        if (other.tag == "Behind" && inventoryManager.hasE == true)
        {
            ui.popUpBar.SetActive(true); // the pop up thing should change 
            inventoryManager.helmUseText.SetActive(false); // this will be removed later
            inventoryManager.invisText.SetActive(false); // this will be removed later 
            inventoryManager.takeDowntext.SetActive(true); // this will be change do game object (icon)
            canKill = true;
            currentEnemy = other.gameObject;
            takeDown = currentEnemy.GetComponent<TakeDown>();
        }
        */

        if (other.CompareTag("Catch") && !Invisible)
        {
            caughtSound.Play();
            lose = true;
            //inventoryManager.takeDowntext.SetActive(false);
           // inventoryManager.invisText.SetActive(false);
            //inventoryManager.dodgeText.SetActive(false);
            // inventoryManager.helmUseText.SetActive(false);
           

            StartCoroutine(LoseGame(.5f));
            //gameObject.SetActive(false);
        }

        if (other.tag == "DodgeUnlocked")
        {
            inDodgeRange = true;
            inventoryManager.dodgeImage.SetActive(true);
        }

        switch (other.tag)
        {
            case "BehindA":
                if (inventoryManager.hasA == true)
                {
                   // ui.popUpBar.SetActive(true); // the pop up thing should change 
                  //  inventoryManager.helmUseText.SetActive(false); // this will be removed later
                //    inventoryManager.invisText.SetActive(false); // this will be removed later 
                  //  inventoryManager.takeDowntext.SetActive(true); // this will be change do game object (icon)
                    canKill = true;
                    currentEnemy = other.gameObject;
                    takeDown = currentEnemy.GetComponent<TakeDown>();
                }
                break;    
            case "BehindE":
               if( inventoryManager.hasE == true)
                {
               //     ui.popUpBar.SetActive(true); // the pop up thing should change 
               //     inventoryManager.helmUseText.SetActive(false); // this will be removed later
                 //   inventoryManager.invisText.SetActive(false); // this will be removed later 
                   // inventoryManager.takeDowntext.SetActive(true); // this will be change do game object (icon)
                    canKill = true;
                    currentEnemy = other.gameObject;
                    takeDown = currentEnemy.GetComponent<TakeDown>();
                }
                break;    
            case "BehindT":
                if (inventoryManager.hasT == true)
                {
             //       ui.popUpBar.SetActive(true); // the pop up thing should change 
           //         inventoryManager.helmUseText.SetActive(false); // this will be removed later
             //       inventoryManager.invisText.SetActive(false); // this will be removed later 
               //     inventoryManager.takeDowntext.SetActive(true); // this will be change do game object (icon)
                    canKill = true;
                    currentEnemy = other.gameObject;
                    takeDown = currentEnemy.GetComponent<TakeDown>();
                }
                break;

            case "TakeDownItemE":

                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasE = true;
                    pickupSound.Play();

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
                    pickupSound.Play();

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
                    pickupSound.Play();

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
                    pickupSound.Play();

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
                    pickupSound.Play();

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
                    pickupSound.Play();

                    inventoryManager.gemCounterT++;
                    inventoryManager.ShowAmount(inventoryManager.gemTextT, inventoryManager.gemCounterT, ref inventoryManager.hasGemT);
                    Destroy(other.gameObject);
                   
                    StartCoroutine(waitToFalse(0.5f));
                }

                break;

            case "Helm":

                if (!hasPickedUpItem) // Check if the Helm was already picked up
                if (!hasPickedUpItem) // Check if the Helm was already picked up
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasHelm = true;
                    pickupSound.Play();
                  //  inventoryManager.helmUseText.SetActive(true);

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
           //inventoryManager.takeDowntext.SetActive(false);
            ui.popUpBar.SetActive(false);

            canKill = false;
            takeDown.dead = false;
            currentEnemy = null;
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            hasPickedUpItem = false; // Reset flag when leaving
            Destroy(other.gameObject);
        }

        if (other.tag == "DodgeUnlocked")
        {
            inDodgeRange = false;
            inventoryManager.dodgeImage.SetActive(false);
        }
    }

    IEnumerator waitToFalse(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hasPickedUpItem = false ;
    }
}