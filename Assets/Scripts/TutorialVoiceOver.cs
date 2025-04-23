using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVoiceOver : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip sound1;
    public AudioClip sound2;
    public AudioClip sound3;
    public AudioClip sound4;
    public AudioClip sound5;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Collider1":
                Debug.Log("Test 1");
                audioSource.PlayOneShot(sound1);
                break;

            case "Collider2":
                Debug.Log("Test 2");
                audioSource.PlayOneShot(sound2);
                break;

            case "Collider3":
                Debug.Log("Test 3");
                audioSource.PlayOneShot(sound3);
                break;

            case "Collider4":
                Debug.Log("Test 4");
                audioSource.PlayOneShot(sound4);
                break;

            case "Collider5":
                Debug.Log("Test 5");
                audioSource.PlayOneShot(sound5);
                break;

            default:
                break;
        }

        /*if (other.tag == "Collider1")
        {
            Debug.Log("Test 1");
            audioSource.PlayOneShot(sound1);
        }
        if(other.tag == "Collider2")
        {
            Debug.Log("Test 2");
            audioSource.PlayOneShot(sound2);
        }
        if(other.tag == "Collider3")
        {
            Debug.Log("Test 3");
            audioSource.PlayOneShot(sound3);
        }
        if(other.tag == "Collider4")
        {
            Debug.Log("Test 4");
            audioSource.PlayOneShot(sound4);
        }
        if(other.tag == "Collider5")
        {
            Debug.Log("Test 5");
            audioSource.PlayOneShot(sound5);
        }*/
    }

    void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Collider1":
                Debug.Log("Collider disabled 1");
                Destroy(GameObject.Find("nameOfCollider1"));
                break;

            case "Collider2":
                Debug.Log("Collider disabled 2");
                Destroy(GameObject.Find("nameOfCollider2"));
                break;

            case "Collider3":
                Debug.Log("Collider disabled 3");
                Destroy(GameObject.Find("nameOfCollider3"));
                break;

            case "Collider4":
                Debug.Log("Collider disabled 4");
                Destroy(GameObject.Find("nameOfCollider4"));
                break;

            case "Collider5":
                Debug.Log("Collider disabled 5");
                Destroy(GameObject.Find("nameOfCollider5"));
                break;

            default:
                break;
        }

        /*if (other.tag == "Collider1")
        {
            Debug.Log("Collider disabled 1");
            Destroy(GameObject.Find("nameOfCollider1"));
        }
        if(other.tag == "Collider2")
        {
            Debug.Log("Collider disabled 2");
            Destroy(GameObject.Find("nameOfCollider2"));
        }
        if(other.tag == "Collider3")
        {
            Debug.Log("Collider disabled 3");
            Destroy(GameObject.Find("nameOfCollider3"));
        }
        if(other.tag == "Collider4")
        {
            Debug.Log("Collider disabled 4");
            Destroy(GameObject.Find("nameOfCollider4"));
        }
        if(other.tag == "Collider5")
        {
            Debug.Log("Collider disabled 5");
            Destroy(GameObject.Find("nameOfCollider5"));
        }*/
        
    }
}