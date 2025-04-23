using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVoiceOver : MonoBehaviour
{
    public AudioSource sound1;
    public AudioSource sound2;
    public AudioSource sound3;
    public AudioSource sound4;
    public AudioSource sound5;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "VoiceLine1":
                Debug.Log("Test 1");
                sound1.Play();
                break;

            case "VoiceLine2":
                Debug.Log("Test 2");
                sound2.Play();
                sound1.Stop();
                break;

            case "VoiceLine3":
                Debug.Log("Test 3");
                sound3.Play();
                sound2.Stop();
                break;

            case "VoiceLine4":
                Debug.Log("Test 4");
                sound4.Play();
                sound3.Stop();
                break;

            case "VoiceLine5":
                Debug.Log("Test 5");
                sound5.Play();
                sound4.Stop();
                break;

            default:
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "VoiceLine1":
                Debug.Log("Collider disabled 1");
                Destroy(GameObject.Find("VoiceLine1"));
                break;

            case "VoiceLine2":
                Debug.Log("Collider disabled 2");
                Destroy(GameObject.Find("VoiceLine2"));
                break;

            case "VoiceLine3":
                Debug.Log("Collider disabled 3");
                Destroy(GameObject.Find("VoiceLine3"));
                break;

            case "VoiceLine4":
                Debug.Log("Collider disabled 4");
                Destroy(GameObject.Find("VoiceLine4"));
                break;

            case "VoiceLine5":
                Debug.Log("Collider disabled 5");
                Destroy(GameObject.Find("VoiceLine5"));
                break;

            default:
                break;
        }
        
    }
}