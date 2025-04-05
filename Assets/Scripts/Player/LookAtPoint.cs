using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPoint : MonoBehaviour
{
    public GameObject cam;
    public float distanceFromCamera = 7f;

    void Update()
    {
        // Get the direction opposite to the camera's forward vector
        Vector3 oppositeSide = cam.transform.forward;

        transform.position = new Vector3
            (
            cam.transform.position.x + oppositeSide.x * distanceFromCamera,
            transform.position.y,
            cam.transform.position.z + oppositeSide.z * distanceFromCamera
            );

        Vector3 flatForward = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z).normalized;
        if (flatForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(flatForward, Vector3.up);
        }
    }
}