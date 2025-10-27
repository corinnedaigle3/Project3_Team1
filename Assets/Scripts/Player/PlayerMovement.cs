using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Defines the player's movement states
    private enum State
    {
        Normal,
        Rolling,
    }

    // Core UI and visual references
    public UI ui;
    public GameObject vfx;

    [Header("References")]
    // References for player orientation, object transforms, and Rigidbody
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    private Rigidbody rb;

    [Header("Input Actions")]
    // Input system for player actions and movement
    private PlayerInput playerInput;
    private InputSystem inputSystem;

    [Header("BeatTheGame")]
    // Counters for Fury Gems collected (E, A, T)
    public int gemE;
    public int gemA;
    public int gemT;

    [Header("Movement")]
    // Controls player speed, direction, and rolling behavior
    public float moveSpeed;
    public Vector3 moveDirection;
    private Vector3 rollDirection;
    private float rollSpeed;
    private State state;

    [Header("Ground Check")]
    // Ground detection settings for movement and physics
    public float playerHeight;
    public LayerMask whatIsGrounded;
    private bool grounded;
    public float groundDrag;

    [Header("Invisible")]
    // Controls player invisibility and timing logic
    private float invisibleTimer = 0;
    public bool Invisible;
    private bool canRun;

    [Header("Collection")]
    // Tracks which realm items have been collected
    public bool asphodelCollectionItem;
    public bool elysiumCollectionItem;
    public bool tartarusCollectionItem;

    [Header("TakeDown")]
    // TakeDown and enemy-related references
    EnemyBehavior enemyBehavior;
    [HideInInspector] public TakeDown takeDown;
    public bool dead;

    // Fury status tracking
    public bool fury1;
    public bool fury2;
    public bool fury3;

    [Header("Take down behavior")]
    // Tracks kill permissions and enemy references for takedowns
    public bool isTakeDown;
    public bool canKill;
    public GameObject currentEnemy;

    [Header("Inventory")]
    // Player inventory references and item usage flags
    public InventoryManager inventoryManager;
    public bool helmInInventory;
    private bool triggerEnter;
    private bool hasPickedUpItem = false;
    public bool helmUsed;

    [Header("Music")]
    // Audio sources for player actions and events
    public AudioSource takeDownSound;
    public AudioSource pickupSound;
    public AudioSource caughtSound;
    public AudioSource dodgeSound;
    public AudioSource gemSoundE;
    public AudioSource gemSoundA;
    public AudioSource gemSoundT;

    [Header("Slope Handling")]
    // Used for controlling movement on slopes and adjusting drag
    public float maxSlopeAngle;
    public float minSlopeAngle;
    public RaycastHit slopeHit;
    private float angle;

    [Header("Dodge")]
    // Dodge system logic and cooldown timers
    public bool canDodge;
    public bool unlockDodge;
    public bool inDodgeRange;
    public float dodgeTimer;
    public bool isDodging;
    public bool playerIsDodging;
    public float dodgeTimerFade;

    [Header("Other")]
    // Miscellaneous state variables
    public bool lose;
    public gemsChecker theGemChecker;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize references and default values
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
        playerIsDodging = false;

        rb.drag = groundDrag;
        dodgeTimer = 0;
        inDodgeRange = false;

        fury1 = false;
        fury2 = false;
        fury3 = false;
    }

    private void Awake()
    {
        // Set up Input System bindings
        playerInput = GetComponent<PlayerInput>();
        inputSystem = new InputSystem();
        inputSystem.Player.Enable();
        inputSystem.Player.Dash.performed += PlayerRun;
        inputSystem.Player.Dash.canceled += PlayerRun;
        inputSystem.Player.Invisible.performed += Invisibility;
        inputSystem.Player.Dodge.performed += DodgeEnemy;
        inputSystem.Player.TakeDown.performed += TakeDownAction;

        state = State.Normal;
    }

    private void OnDisable()
    {
        // Disable input when the object is disabled
        inputSystem.Player.Disable();
    }

    private void Update()
    {
        // Constantly check for slope movement
        OnSlopeNow();
    }

    private void FixedUpdate()
    {
        // Handles player physics-based movement
        switch (state)
        {
            case State.Normal:
                // Standard movement and input handling
                Vector2 inputVector = inputSystem.Player.Move.ReadValue<Vector2>().normalized;
                Vector3 camRelativeMove = orientation.forward * inputVector.y + orientation.right * inputVector.x;
                camRelativeMove.y = 0;
                moveDirection = camRelativeMove;

                Vector3 moveDirToUse = moveDirection;

                if (OnSlope())
                {
                    moveDirToUse = GetSlopeMoveDirection();
                    rb.drag = groundDrag;
                }

                rb.MovePosition(rb.position + moveDirToUse * moveSpeed * Time.fixedDeltaTime);

                // Smooth rotation toward movement direction
                if (moveDirection != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
                }

                SpeedControl();
                StickToGround();

                // Handle invisibility expiration
                if (invisibleTimer <= 0)
                {
                    ui.popUpBar2.SetActive(false);
                    canRun = false;
                    Invisible = false;
                    helmUsed = false;
                    moveSpeed = 8f;
                    vfx.SetActive(false);
                }

                // Dodge cooldown logic
                if (dodgeTimerFade <= 0)
                {
                    isDodging = true;
                }
                else
                {
                    isDodging = false;
                }

                if (dodgeTimer <= 0)
                {
                    canDodge = true;
                }
                else
                {
                    canDodge = false;
                }

                grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGrounded);

                // Ground and dodge unlock logic
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
                // Handles dodge roll motion and deceleration
                Vector3 dodgeDir = OnSlope() ? GetSlopeMoveDirection() : new Vector3(rollDirection.x, 0f, rollDirection.z).normalized;
                float currentYVel = rb.velocity.y;
                Vector3 dodgeMove = dodgeDir * moveSpeed * rollSpeed * Time.fixedDeltaTime;
                rb.MovePosition(rb.position + dodgeMove);

                float rollSpeedDropMultiplier = 5f;
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;
                float rollSpeedMin = 15f;

                if (rollSpeed < rollSpeedMin)
                {
                    state = State.Normal;
                }

                // Revert invisibility when timer runs out
                if (invisibleTimer <= 0)
                {
                    ui.popUpBar2.SetActive(false);
                    canRun = false;
                    Invisible = false;
                    vfx.SetActive(false);
                    helmUsed = false;
                    moveSpeed = 8f;
                }

                // Dodge cooldown logic
                if (dodgeTimerFade <= 0)
                {
                    isDodging = true;
                }
                else
                {
                    isDodging = false;
                }

                if (dodgeTimer <= 0)
                {
                    canDodge = true;
                }
                else
                {
                    canDodge = false;
                }

                // Ground and slope physics while rolling
                if (grounded)
                {
                    rb.drag = groundDrag;
                    unlockDodge = true;
                }
                else if (!grounded && !OnSlope())
                {
                    rb.drag = 0;
                    unlockDodge = false;
                    rb.AddForce(Vector3.down * 10f, ForceMode.Acceleration);
                }

                if (OnSlope())
                {
                    moveDirToUse = GetSlopeMoveDirection();
                    rb.drag = groundDrag;
                    rb.AddForce(-slopeHit.normal * 50f, ForceMode.Force);
                }

                invisibleTimer -= Time.deltaTime;
                dodgeTimer -= Time.deltaTime;
                dodgeTimerFade -= Time.deltaTime;
                break;
        }
    }

    // Handles player takedown and gem-related "Q" actions
    public void TakeDownAction(InputAction.CallbackContext context)
    {
        if (canKill == true && currentEnemy != null && context.performed)
        {
            isTakeDown = true;
            Debug.Log("Q is pressed");
            if (takeDown != null)
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

        if (theGemChecker != null && theGemChecker.canPressQ)
        {
            theGemChecker.qPressed = true;
        }
    }

    // Controls overall player movement speed
    private void SpeedControl()
    {
        if (OnSlope())
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
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

    // Handles sprinting input
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

    // Handles dodge/roll behavior when dodge key is pressed
    private void DodgeEnemy(InputAction.CallbackContext context)
    {
        if (context.performed && canDodge == true && unlockDodge == true && inDodgeRange == true)
        {
            playerIsDodging = true;
            rollDirection = moveDirection;
            rollSpeed = 17f;
            state = State.Rolling;
            dodgeTimer = 8.9f;
            dodgeSound.Play();
            dodgeTimerFade = 4.8f;
        }
    }

    // Activates invisibility if the player has a helm
    private void Invisibility(InputAction.CallbackContext context)
    {
        if (context.performed && inventoryManager.hasHelm == true)
        {
            Invisible = true;
            canRun = true;
            vfx.SetActive(true);
            invisibleTimer = 5f;
            helmUsed = true;
            inventoryManager.helmcounter -= 1;
            inventoryManager.ShowAmount(inventoryManager.helmText, inventoryManager.helmcounter, ref inventoryManager.hasHelm);
        }
    }

    // Checks if the player is on a slope
    public bool OnSlope()
    {
        float sphereCastRadius = 0.3f;
        float rayLength = playerHeight * 0.5f + 0.5f;
        Vector3 origin = transform.position;

        if (Physics.SphereCast(origin, sphereCastRadius, Vector3.down, out slopeHit, rayLength))
        {
            angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle > minSlopeAngle && angle != 0;
        }

        return false;
    }

    // Calculates correct slope movement direction
    public Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    // Adds extra downward force when on slope to maintain ground contact
    private void OnSlopeNow()
    {
        if (OnSlope())
        {
            Vector3 slopeMove = GetSlopeMoveDirection() * moveSpeed;
            rb.AddForce(slopeMove, ForceMode.Acceleration);

            if (rb.velocity.y <= 0.1f)
            {
                rb.AddForce(-slopeHit.normal * 100f, ForceMode.Force);
            }
        }
    }

    // Keeps player grounded with downward force when not on slope
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

    // Coroutine to trigger lose screen after delay
    IEnumerator LoseGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ui.LoadLose();
    }

    // Handles all item pickups, takedowns, and collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Catch") && !Invisible)
        {
            caughtSound.Play();
            lose = true;
            StartCoroutine(LoseGame(.5f));
        }

        if (other.tag == "DodgeUnlocked")
        {
            inDodgeRange = true;
            inventoryManager.dodgeImage.SetActive(true);
        }

        switch (other.tag)
        {
            // Takedown zone detection per fury type
            case "BehindA":
                if (inventoryManager.hasA == true)
                {
                    canKill = true;
                    currentEnemy = other.gameObject;
                    takeDown = currentEnemy.GetComponent<TakeDown>();
                }
                break;
            case "BehindE":
                if (inventoryManager.hasE == true)
                {
                    canKill = true;
                    currentEnemy = other.gameObject;
                    takeDown = currentEnemy.GetComponent<TakeDown>();
                }
                break;
            case "BehindT":
                if (inventoryManager.hasT == true)
                {
                    canKill = true;
                    currentEnemy = other.gameObject;
                    takeDown = currentEnemy.GetComponent<TakeDown>();
                }
                break;

            // Item pickups for takedown items and gems
            case "TakeDownItemE":
                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasE = true;
                    pickupSound.Play();
                    Debug.Log("Take down item picked up");
                    inventoryManager.takeDownItemCounterE = 1;
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
                    inventoryManager.takeDownItemCounterA = 1;
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
                    inventoryManager.takeDownItemCounterT = 1;
                    inventoryManager.ShowAmount(inventoryManager.takeDownItemTextT, inventoryManager.takeDownItemCounterT, ref inventoryManager.hasT);
                    Destroy(other.gameObject);
                    StartCoroutine(waitToFalse(0.5f));
                }
                break;

            case "GemE":
                if (!hasPickedUpItem)
                {
                    gemSoundE.Play();
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
                    gemSoundA.Play();
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
                    gemSoundT.Play();
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
                if (!hasPickedUpItem)
                {
                    hasPickedUpItem = true;
                    inventoryManager.hasHelm = true;
                    pickupSound.Play();
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

    // Resets states and UI when exiting triggers
    IEnumerator waitToFalse(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hasPickedUpItem = false ;
    }
}