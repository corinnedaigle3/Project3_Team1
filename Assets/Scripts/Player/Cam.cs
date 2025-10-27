using UnityEngine;
using UnityEngine.InputSystem;

public class Cam : MonoBehaviour
{
    [Header("References")]
    // The player's orientation object (used to align movement direction with camera view)
    public Transform orientation;

    // Reference to the player's main transform (position in world space)
    public Transform player;

    // Reference to the visible player object (model/mesh that rotates visually)
    public Transform playerObj;

    // Reference to the player's Rigidbody component
    public Rigidbody rb;

    // Direction of the player's movement input (calculated from input axes)
    private Vector3 inputDir;

    // Direction from the camera to the player (used to orient movement)
    private Vector3 viewDir;

    // Raw input values for horizontal and vertical movement
    float horizontalInput;
    float verticalInput;

    // How quickly the player object rotates toward the desired direction
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // Lock the mouse cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Hide the mouse cursor
        Cursor.visible = false;
    }

    // Reads raw player movement input (WASD / arrow keys)
    private void MyInput()
    {
        // Get horizontal (A/D or Left/Right) input
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Get vertical (W/S or Up/Down) input
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the direction from the camera to the player on the XZ plane
        viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);

        // Align the orientation object to face the same direction as the camera's view direction
        orientation.forward = viewDir.normalized;

        // Calculate input direction relative to the camera orientation
        inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // If there is input (player is moving), smoothly rotate the player object toward that direction
        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(
                playerObj.forward,                // current facing direction
                inputDir.normalized,              // target direction
                Time.fixedDeltaTime * rotationSpeed // rotation speed factor
            );
        }

        // Continuously read player input every frame
        MyInput();
    }
}
