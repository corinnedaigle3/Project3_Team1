using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{  
    PlayerMovement playerMovement;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovement.playerIsDodging == true) 
        {
            playerMovement.playerIsDodging = false;
            Debug.Log("Player dodging");
            animator.SetBool("walking", false);
            animator.SetBool("dodge", true);
            animator.SetBool("takedown", false);
            return;
        } 
        if(playerMovement.isTakeDown == true) 
        {
            playerMovement.isTakeDown = false;
            Debug.Log("Player takedown");
            animator.SetBool("walking", false);
            animator.SetBool("dodge", false);
            animator.SetBool("takedown", true);
            return;
        }
        if(playerMovement.moveDirection != Vector3.zero) 
        {
            Debug.Log("Player moving");
            animator.SetBool("walking", true);
            animator.SetBool("dodge", false);
            animator.SetBool("takedown", false);
        }
        else
        {
            Debug.Log("Player not moving");
            animator.SetBool("walking", false);
            animator.SetBool("dodge", false);
            animator.SetBool("takedown", false);
        }
    }
}

