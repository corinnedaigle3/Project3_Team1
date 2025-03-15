using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
   private enum State {
        Normal,
        Rolling,
    }

    [Header("Movement")]
    public float moveSpeed;
    private float horizontalInput;
    private float verticalInput;
    Vector3 moveDirection; 
    private Vector3 rollDirection;
    private float rollSpeed;
    private State state;

    [Header("Parry")]
    public Collider parryCollider;
    private int parryFrameCount;

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

   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        moveSpeed = 8f;
    }

    private void Awake()
    {
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

                //Speed Controll
                rb.velocity = moveDirection * moveSpeed;


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
                //Invisability();
                //ThrowObj();
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = 12f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 8f;
        }
    }

    private void DodgeEnemy()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rollDirection = moveDirection;
            rollSpeed = 24f;
            state = State.Rolling;
        }
    }
}
