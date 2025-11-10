using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class LineOfSight : MonoBehaviour
{
    // Reference to the player GameObject
    public GameObject player;
    // Reference to the player's movement script
    public PlayerMovement playerMovement;
    // Whether the enemy can currently chase the player
    public bool canChase;

    [Header("Raycast Settings")]
    // Layer that represents obstacles (used to block line of sight)
    public LayerMask obstacleLayer;
    // Layer that represents the player (used to detect if player is in view range)
    public LayerMask playerLayer;

    // How far the enemy can see
    public float radius;
    // The field of view angle (spread of vision)
    [Range(0, 360)]
    public float angle;

    private void Start()
    {
        // Find the player in the scene and get its PlayerMovement component
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Only perform vision checks if the player is not invisible
        if (!playerMovement.Invisible)
        {
            FieldOfViewCheck();
        }
        else
        {
            // If player is invisible, enemy cannot chase
            canChase = false;
        }
    }

    /// <summary>
    /// Checks if the player is within the enemy's field of view and not obstructed by obstacles.
    /// </summary>
    private void FieldOfViewCheck()
    {
        // Collect all colliders within the radius that are on the player layer
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, playerLayer);

        // If at least one player collider was detected
        if (rangeChecks.Length != 0)
        {
            // Get the transform of the first detected target (player)
            Transform target = rangeChecks[0].transform;

            // Calculate direction from enemy to player
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Check if the player is within the vision cone (angle/2 on each side)
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                // Measure how far the player is
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // Draw a debug ray in the editor to visualize the line of sight
                Debug.DrawRay(transform.position, directionToTarget * distanceToTarget, Color.red);

                // Create an array to store raycast hits
                RaycastHit[] hits = new RaycastHit[10];

                // Cast a ray to check if any obstacle blocks the view between enemy and player
                int hitCount = Physics.RaycastNonAlloc(
                    transform.position,
                    directionToTarget,
                    hits,
                    distanceToTarget,
                    obstacleLayer,
                    QueryTriggerInteraction.Collide
                );

                // If nothing blocks the view and the player isn’t invisible → enemy can chase
                if (hitCount == 0 && !playerMovement.Invisible)
                {
                    canChase = true;
                }
                else
                {
                    canChase = false;
                }
            }
            else
            {
                // Player is outside of vision cone
                canChase = false;
            }
        }
        else if (canChase)
        {
            // If player leaves detection range, stop chasing
            canChase = false;
        }
    }

}
