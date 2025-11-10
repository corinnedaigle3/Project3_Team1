using UnityEngine;

/// <summary>
/// Controls the catchable object's behavior when interacting with the player.
/// Disables its collider if the player becomes invisible to prevent being detected or caught.
/// </summary>
public class CatchBehave : MonoBehaviour
{
    public Collider theCatch;  // Reference to this catchable object

    public PlayerMovement p;    // Reference to the player's movement/ability script

    void Start()
    {
        // Cache references for efficiency
        theCatch = GetComponent<Collider>();
        p = GameObject.Find("Player").GetComponent<PlayerMovement>();
     
    }


    void Update()
    {
        GameObject.Find("Player");

        // Disable collider if the player is invisible (so this object can't detect or interact with them)
        theCatch.enabled = !p.Invisible;
    }
}
