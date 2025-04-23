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
            // Delay the teleportation
            StartCoroutine(DelayedTeleport(other));
        }

        Cinemachine.CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.OnTargetObjectWarped(
            player.transform, destination.transform.position - player.transform.position);
    }

    private IEnumerator DelayedTeleport(Collider other)
    {
        yield return null; // Wait for one frame
        other.transform.position = destination.transform.position;
    }
}
