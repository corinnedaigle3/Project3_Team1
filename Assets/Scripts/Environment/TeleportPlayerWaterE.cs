using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerWaterE : MonoBehaviour
{
    public GameObject destination;
    PlayerMovement player;
    Cam cam;

    private void Awake()
    {
        if (gameObject.tag == "EnemyT" || gameObject.tag == "EnemyA" || gameObject.tag == "EnemyE")
        {
            player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Delay the teleportation
            StartCoroutine(DelayedTeleport(other));
        }
    }

    private IEnumerator DelayedTeleport(Collider other)
    {
        //Wait for one frame
        yield return null;
        other.transform.position = destination.transform.position;
    }
}
