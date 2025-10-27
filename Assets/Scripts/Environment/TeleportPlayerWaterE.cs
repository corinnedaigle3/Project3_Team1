using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Handles teleporting the player when entering a trigger (for example, falling into water)
// Also notifies the level manager if the player has fallen
public class TeleportPlayerWaterE : MonoBehaviour
{
    // The location the player (or other objects) will be teleported to
    public GameObject destination;

    // Reference to the player's movement component
    PlayerMovement player;

    // Reference to the level manager that tracks state like whether the player fell
    public TarturusManager levelManager;

    private void Awake()
    {
        // If this object has an enemy tag, find the Player object in the scene
        // and get its PlayerMovement script.
        if (gameObject.tag == "EnemyT" || gameObject.tag == "EnemyA" || gameObject.tag == "EnemyE")
        {
            player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        }

        // If the current scene is "Tarturus", find the TartarusManager
        // (used to handle logic like respawning or state tracking)
        if (SceneManager.GetActiveScene().name == "Tarturus")
        {
            levelManager = GameObject.Find("TartarusManager").GetComponent<TarturusManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // When another collider enters this trigger
        if (other.CompareTag("Player"))
        {
            // Begin teleporting the player after a short delay (via coroutine)
            StartCoroutine(DelayedTeleport(other));

            // If a level manager is active, notify it that the player fell
            if (levelManager != null)
            {
                Debug.Log("Player fell is: ");
                levelManager.playerFell = true;
            }
        }
    }

    private IEnumerator DelayedTeleport(Collider other)
    {
        // Wait a frame before teleporting (can prevent timing issues)
        yield return null;

        // Store the old and new positions
        Vector3 oldPos = other.transform.position;
        Vector3 newPos = destination.transform.position;

        // Move the player to the destination
        other.transform.position = newPos;

        // Try to get the player's Rigidbody to reset physics behavior
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Stop any current movement or rotation
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Temporarily disable physics to safely move the object
            rb.isKinematic = true;
            rb.MovePosition(newPos);

            // Wait for the physics system to update before re-enabling physics
            yield return new WaitForFixedUpdate();

            // Re-enable normal physics interaction
            rb.isKinematic = false;
        }
        else
        {
            // If there’s no Rigidbody, just directly move the transform
            other.transform.position = newPos;
        }

        // Update Cinemachine’s camera target position after teleport
        // (prevents sudden camera snapping or disorientation)
        var brain = Cinemachine.CinemachineCore.Instance.GetActiveBrain(0);
        if (brain != null && brain.ActiveVirtualCamera != null)
        {
            brain.ActiveVirtualCamera.OnTargetObjectWarped(other.transform, newPos - oldPos);
        }
    }
}
