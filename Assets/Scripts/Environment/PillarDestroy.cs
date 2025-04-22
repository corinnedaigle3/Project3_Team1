using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PillarDestroy : MonoBehaviour
{
    public AudioSource sfx;
    public Animator animator;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SelfDestuct(3f));
        }
    }
  

    IEnumerator SelfDestuct (float waittaime)
    {
        yield return new WaitForSeconds (2f);
        animator.SetTrigger("Fall");
        // Enabme Sfx if it is going to be used 
        //sfx.Play();

        yield return new WaitForSeconds (waittaime);
        Destroy(gameObject);
    }
}
