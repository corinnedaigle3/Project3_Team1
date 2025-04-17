using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerWaterE : MonoBehaviour
{
    public GameObject destination;
    private void Awake()
    {
        destination = GameObject.FindWithTag("Spawn");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = destination.transform.position;
        }
    }
}
