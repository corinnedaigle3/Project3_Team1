using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerWaterE : MonoBehaviour
{
    public GameObject destination; // Drag the destination GameObject here in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the colliding object is tagged as "Player"
        {
            // Teleport the player to the destination
            other.transform.position = destination.transform.position;
        }
    }
}
