using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportPlayerWaterE : MonoBehaviour
{
    public GameObject destination;
    PlayerMovement player;
    public TarturusManager levelManager;

    private void Awake()
    {
        if (gameObject.tag == "EnemyT" || gameObject.tag == "EnemyA" || gameObject.tag == "EnemyE")
        {
            player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        }
        if (SceneManager.GetActiveScene().name == "Tarturus")
        {
            levelManager = GameObject.Find("TarturusManager").GetComponent<TarturusManager>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Delay the teleportation
            StartCoroutine(DelayedTeleport(other));
            if (levelManager != null)
            {
                levelManager.playerFell = true;
            }
        }
    }

    private IEnumerator DelayedTeleport(Collider other)
    {
        //wait one second
        yield return null;

        Vector3 oldPos = other.transform.position;
        Vector3 newPos = destination.transform.position;

        //teleport player
        other.transform.position = destination.transform.position;

        //Reset rigid body
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            //disable physics
            rb.isKinematic = true;
            rb.MovePosition(newPos);
            //gives phsyics time to update
            yield return new WaitForFixedUpdate();
            //enaable physics
            rb.isKinematic = false;
        }
        else
        {
            other.transform.position = newPos;
        }

        //Cinemachine to snap to the player after teleport
        var brain = Cinemachine.CinemachineCore.Instance.GetActiveBrain(0);
        if (brain != null && brain.ActiveVirtualCamera != null)
        {
            brain.ActiveVirtualCamera.OnTargetObjectWarped(other.transform, newPos - oldPos);
        }
    }
}