using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    private GameObject lookAtPoint;
    public float shootingForce = 1000f;

    private void Awake()
    {
        lookAtPoint = GameObject.Find("LookAtPoint");
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Vector3 shootingDirection = lookAtPoint.transform.forward;

        rb.AddForce(shootingDirection * shootingForce);

        Debug.Log("Shooting Direction is " + shootingDirection);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);

        }
        StartCoroutine(SelfDestruction(.5f));
    }

    IEnumerator SelfDestruction(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
