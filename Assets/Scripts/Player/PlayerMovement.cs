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
    private bool invisibleTime;

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
    private Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        moveSpeed = 8f;
        canDash = false;
        Invisible = false;
        asphodelCollectionItem = false;
        elysiumCollectionItem = false;
        tartarusCollectionItem = false;
        invisibleTime = false;
        takeDowntext.SetActive(false);

        fury1 = false;
        fury2 = false;
        fury3 = false;
    }

    private void Awake()
    {
        state = State.Normal;
        inventory = new Inventory(UseItem);
        uiInventory.SetInventory(inventory);
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

                //check if player is on ground
                if (grounded)
                {
                    rb.drag = groundDrag;
                }
                else
                {
                    rb.drag = 0;
                }

                DashPlayer();
                DodgeEnemy();
                Invisibility();
                invisibleTimer -= Time.deltaTime;

                // makes the enemy stop and destroys the collider when pressing Q
                if (canKill == true && currentEnemy != null && Input.GetKeyDown(KeyCode.Q))
                {
                    Debug.Log("Q is pressed");
                    takeDown = currentEnemy.GetComponent<TakeDown>();
                    if (takeDown != null) // Add a null check
                    {
                        takeDown.dead = true;
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

                DashPlayer();
                DodgeEnemy();
                Invisibility();
                invisibleTimer -= Time.deltaTime;

                if (rollSpeed < rollSpeedMin)
                {
                    state = State.Normal;
                }
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
        if (invisibleTime == true)
        {
            Invisible = true;
            canDash = true;
            invisibleTimer = 3f;
        }

        if (invisibleTimer <= 0)
        {
            canDash = false;
            Invisible = false;
            moveSpeed = 8f;
            invisibleTime = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Behind")
        {
            //take down enemy text
            takeDowntext.SetActive(true);
            canKill = true;
            currentEnemy = other.gameObject;
        }

        if (other.CompareTag("Enemy") || other.CompareTag( "Fury"))
        {
            other.gameObject.GetComponentInParent<EnemyBehavior>().playerLose = true;
            transform.LookAt(other.transform.position);
            gameObject.GetComponent<PlayerMovement>().enabled = false;
        }

        //ItemWorld itemWorld = GetComponent<ItemWorld>();
        //itemWorld.GetComponent<Collider>();

        if (other.tag == "TakeDownItemE")
        {
            Destroy(other.gameObject);
            inventory.AddItem(new Item { itemType = Item.ItemType.TakeDownItemE, amount = 1});
        }

        if (other.tag == "TakeDownItemA")
        {
            inventory.AddItem(new Item { itemType = Item.ItemType.TakeDownItemA, amount = 1});
            Destroy(other.gameObject);
        }

        if (other.tag == "TakeDownItemT")
        {
            inventory.AddItem(new Item { itemType = Item.ItemType.TakeDownItemT, amount = 1});
            Destroy(other.gameObject);
        }

        if (other.tag == "GemE")
        {
            gemE++;
            inventory.AddItem(new Item { itemType = Item.ItemType.Gem1, amount = 1});
            Destroy(other.gameObject);
        }

        if (other.tag == "GemA")
        {
            gemA++;
            inventory.AddItem(new Item { itemType = Item.ItemType.Gem2, amount = 1});
            Destroy(other.gameObject);
        }

        if (other.tag == "GemT")
        {
            gemT++;
            inventory.AddItem(new Item { itemType = Item.ItemType.Gem3, amount = 1});
            Destroy(other.gameObject);
        }

        if (other.tag == "Helm")
        {
            Destroy(other.gameObject);
            inventory.AddItem(new Item { itemType = Item.ItemType.Helm, amount = 1});
        }

    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Helm:
                invisibleTime = true; 
                Invisibility();
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Helm, amount = 1 });
                break;
            case Item.ItemType.Gem1:
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Gem1, amount = 1 });
                break;
            case Item.ItemType.Gem2:
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Gem2, amount = 1 });
                break;
            case Item.ItemType.Gem3:
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Gem3, amount = 1 });
                break;
            case Item.ItemType.TakeDownItemE:
                inventory.RemoveItem(new Item { itemType = Item.ItemType.TakeDownItemE, amount = 1 });
                break;
            case Item.ItemType.TakeDownItemA:
                inventory.RemoveItem(new Item { itemType = Item.ItemType.TakeDownItemA, amount = 1 });
                break;
            case Item.ItemType.TakeDownItemT:
                inventory.RemoveItem(new Item { itemType = Item.ItemType.TakeDownItemT, amount = 1 });
                break;
        }
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
    }
}