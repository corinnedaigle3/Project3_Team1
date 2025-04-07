using UnityEngine;
using UnityEngine.InputSystem;

public class Cam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    [SerializeField] private InputActionAsset PlayerControls;
    private InputAction lookAction;
    //private Vector2 lookInput;
    private Vector3 inputDir;
    private Vector3 viewDir;

    float horizontalInput;
    float verticalInput;

    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Awake()
    {
        lookAction = PlayerControls.FindActionMap("Player").FindAction("Look");

        lookAction.performed += context => inputDir = context.ReadValue<Vector2>();
        lookAction.canceled += context => inputDir = Vector2.zero;

        lookAction.performed += context => viewDir = context.ReadValue<Vector2>();
        lookAction.canceled += context => viewDir = Vector2.zero;
    }

    private void OnEnable()
    {
        lookAction.Enable();
    }

    private void OnDisable()
    {
        lookAction.Disable();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    // Update is called once per frame
    void Update()
    {
        viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
        inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        MyInput();
    }
}
