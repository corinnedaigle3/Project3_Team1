using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryController : MonoBehaviour
{
    public float parryWindowDuration = 0.2f; // Duration of the parry window in seconds

    private bool isParrying = false;
    private float parryTimer = 0f;
    private bool isEnemyAttacking = false;

    void Update()
    {
        if (isEnemyAttacking)
        {
            if (Input.GetKeyDown(KeyCode.RightShift) && parryTimer <= parryWindowDuration)
            {
                Parry();
            }
            else
            {
                // GameOver Screen
            }
        }
    }

    // Called by the enemy's attack animation
    public void StartEnemyAttack()
    {
        isEnemyAttacking = true;
        parryTimer = 0f;
    }

    // Called by the enemy's attack animation
    public void EndEnemyAttack()
    {
        isEnemyAttacking = false;
    }

    void Parry()
    {
        // Play parry animation
        Debug.Log("Parried!");
        // Deflect the attack
        // Disable the attack's collider or use a different method
        // Optionally, stun or damage the enemy
        isParrying = true;
        parryTimer = 0f;
        // Set a timer to reset parry state after a short time
        Invoke("ResetParry", 0.5f);
    }

    void ResetParry()
    {
        isParrying = false;
    }
}
