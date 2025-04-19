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
        if(playerMovement.moveDirection != Vector3.zero) //if player is moving set walking animation to true
        {
            Debug.Log("Player moving");
            animator.SetBool("walking", true);
        }
        else
        {
            Debug.Log("Player not moving");
            animator.SetBool("walking", false);
        }
        
    }
}
