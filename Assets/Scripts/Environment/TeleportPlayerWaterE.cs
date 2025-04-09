using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerWaterE : MonoBehaviour
{
    public GameObject destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = destination.transform.position;
        }
    }
}
