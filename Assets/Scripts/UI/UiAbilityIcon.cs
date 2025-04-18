using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAbilityIcon : MonoBehaviour
{ 
    // try doing something for icon 
   public PlayerMovement player;
    public Image image;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<PlayerMovement>();

        }
        // If player CAN dodge, set fillAmount to 0 (ability ready)
        if (player.canDodge)
        {
            image.fillAmount = 0f;
            Debug.Log("Fill amount is " + image.fillAmount);
        }
        else
        {
            // If cooldown is active, reduce fillAmount gradually to 0 over dodgeTimer
            if (player.dodgeTimer > 0)
            {
                image.fillAmount = player.dodgeTimer / 5f;
                Debug.Log("Fill amount is " + image.fillAmount);

            }
        }
    }
}
