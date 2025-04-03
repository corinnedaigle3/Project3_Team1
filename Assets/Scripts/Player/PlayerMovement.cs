using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
   private enum State 
    {
        Normal,
        Rolling,
    }

    public static GameObject playerInstance;

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

    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    
    private Rigidbody rb;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGrounded;
    private bool grounded;
    public float groundDrag;

    [Header("Invisible")]
    private float invisibleTimer = 0;
    public bool Invisible;
    private bool canDash;
    //private bool invisibleTime;

    [Header("Pickup and Throw")]

    [Header("Collection")]
    public bool asphodelCollectionItem;
    public bool elysiumCollectionItem;
    public bool tartarusCollectionItem;

    [Header("TakeDown")]
    EnemyBehavior enemyBehavior;
    public GameObject takeDowntext;
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
    public int amount;
    private bool triggerEnter;
    private bool hasPickedUpItem = false;

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
        takeDowntext = GameObject.Find("TakeDownText");
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        moveSpeed = 8f;
        canDash = false;
        Invisible = false;
        asphodelCollectionItem = false;
        elysiumCollectionItem = false;
        tartarusCollectionItem = false;
        //invisibleTime = false;
        //takeDowntext.SetActive(false);

        amount = 0;

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
        state = State.Normal;
        //inventory = new Inventory(UseItem);
        //uiInventory.SetInventory(inventory);
    }

    // Update is called once per frame
    void Update()
    {
       
        switch (state)
        {
            case State.Normal:
                grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGrounded);

                //inventoryManager = GetComponent<InventoryManager>();

                MyInput();
                SpeedControl();
                DashPlayer();
                DodgeEnemy();
                Invisibility();
               // ShowAmount();

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
                MovePlayer();
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
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        transform.rotation = Quaternion.LookRotation(moveDirection);
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

    private void DashPlayer() // are we using this ????? Player can dash when Invisable
    {
        if (canDash == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                moveSpeed = 12f;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                moveSpeed = 8f;
            }
        }
    }

    private void DodgeEnemy() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rollDirection = moveDirection;
            rollSpeed = 18f;
            state = State.Rolling;
        }
    }

    private void Invisibility()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && inventoryManager.hasHelm == true)
        {
            Invisible = true;
            canDash = true;
            invisibleTimer = 6f;
            amount -= 1;
        }

        if (invisibleTimer <= 0)
        {
            canDash = false;
            Invisible = false;
            moveSpeed = 8f;
        }
    }
    /*
    private void ShowAmount()
    {
        if (amount == 0)
        {
            inventoryManager.hasHelm = false;
            inventoryManager.helmText1.gameObject.SetActive(false);
            inventoryManager.helmText2.gameObject.SetActive(false);
            inventoryManager.helmText3.gameObject.SetActive(false);
        }
        else if (amount == 1)
        {
            inventoryManager.helmText1.gameObject.SetActive(true);
            inventoryManager.helmText2.gameObject.SetActive(false);
            inventoryManager.helmText3.gameObject.SetActive(false);
        }
        else if (amount == 2)
        {
            inventoryManager.helmText1.gameObject.SetActive(false);
            inventoryManager.helmText2.gameObject.SetActive(true);
            inventoryManager.helmText3.gameObject.SetActive(false);
        }
        else if (amount == 3)
        {
            inventoryManager.helmText2.gameObject.SetActive(false);
            inventoryManager.helmText3.gameObject.SetActive(true);
            inventoryManager.helmText3.gameObject.SetActive(false);
        }
        else
        {
            inventoryManager.helmText1.gameObject.SetActive(false);
            inventoryManager.helmText2.gameObject.SetActive(false);
            inventoryManager.helmText3.gameObject.SetActive(false);
        }
    }
*/


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("colliding ");
        if (other.tag == "Behind")
        {

             takeDowntext.SetActive(true);
             canKill = true;
             currentEnemy = other.gameObject;
            takeDown = currentEnemy.GetComponent<TakeDown>();


        }

        if (other.CompareTag("EnemyE") || other.CompareTag( "Fury") || other.CompareTag("EnemyA") || other.CompareTag("EnemyA"))
        {
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
                

                /*
                if (!triggerEnter && amount <= 3)
                {
                    amount += 1;
                    triggerEnter = true;
                }

                */
                break;

             default:
                break;
            
        }
        /*
        if (other.tag == "TakeDownItemE")
        {
            inventoryManager.hasApple = true;
            Destroy(other.gameObject);
        }

        if (other.tag == "TakeDownItemA")
        {
            inventoryManager.hasSkull = true;
            Destroy(other.gameObject);
        }

        if (other.tag == "TakeDownItemT")
        {
            inventoryManager.hasFireFlower = true;
            Destroy(other.gameObject);
        }

        if (other.tag == "GemE")
        {
            inventoryManager.hasGem1 = true;
            Destroy(other.gameObject);
        }

        if (other.tag == "GemA")
        {
            inventoryManager.hasGem2 = true;
            Destroy(other.gameObject);
        }

        if (other.tag == "GemT")
        {
            inventoryManager.hasGem3 = true;
            Destroy(other.gameObject);
        }

        if (other.tag == "Helm")
        {
            inventoryManager.hasHelm = true;
            if (!triggerEnter && amount <= 3)
            {
                amount += 1;
                triggerEnter = true;
            }
        }
        */

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Behind")
        {
            //take down enemy text
            takeDowntext.SetActive(false);
            canKill = false;
            currentEnemy = null;
        }

       
            hasPickedUpItem = false; // Reset flag when leaving
        

        /*
        if (other.tag == "Helm")
        {
            triggerEnter = false;
            Destroy(other.gameObject);
        }
        */
    }
}