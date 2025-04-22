using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class LineOfSight : MonoBehaviour
{
    public GameObject player;
    public PlayerMovement playerMovement;
    public bool canChase;


    [Header("Raycast Settings")]
    public LayerMask obstacleLayer; // Layer for obstacles 
    public LayerMask playerLayer;

    public float radius;
    [Range(0,360)]
    public float angle; //meant to type "angle"

    private void Start()
    {
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!playerMovement.Invisible) {
            FieldOfViewCheck();
        }else
        {
            canChase = false;
        }

    }

    private void FieldOfViewCheck()
    {
        // collect all colliders 
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, playerLayer);

        if(rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                Debug.DrawRay(transform.position, directionToTarget * distanceToTarget, Color.red);

                // RaycastNonAlloc version
                RaycastHit[] hits = new RaycastHit[10];

                int hitCount = Physics.RaycastNonAlloc(transform.position,
                    directionToTarget, hits, distanceToTarget, obstacleLayer,
                    QueryTriggerInteraction.Collide);

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
                canChase = false;
                
            }
        }
        else if (canChase)
        {
            canChase = false;
            
        }
    }

}
