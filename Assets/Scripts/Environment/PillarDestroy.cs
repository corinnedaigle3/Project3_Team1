using System.Collections;
using UnityEngine;

public class PillarDestroy : MonoBehaviour
{
    public AudioSource sfx;
    public float duration = 5f;
    public TarturusManager manager;
    private Vector3 originalPosition;

    private void Awake()
    {
        manager = GameObject.Find("TarturusManager").GetComponent<TarturusManager>();
        originalPosition = transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            StartCoroutine(SelfDestuct());
        }
    }
    private void OnEnable()
    {
        Debug.Log("Object enabled");
        transform.position = originalPosition;
    }

    IEnumerator SelfDestuct()
    {
        yield return new WaitForSeconds(2f);
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, -6f, startPosition.z); // Keep X and Z, just change Y
        sfx.Play();
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        // Enabme Sfx if it is going to be used 
        //
        Debug.Log("Disable Object");
        gameObject.SetActive(false);
    }
}
