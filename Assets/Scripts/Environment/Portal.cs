using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string writeNameOfScene; // user should type the scene to load  
    public GameObject gameManager; // game manager reference 
    public Light l;


    private void Awake()
    {
        gameManager = GameObject.Find("GameManager");
    }

    private void Update()
    {
        if (l != null && gameManager.GetComponent<GameManger>().win)
        {
            l.enabled = true; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag != "win")
        {
            if (other.CompareTag("Player"))
            {

                SceneManager.LoadScene(writeNameOfScene);

            }

        } else
        {
            if (other.CompareTag("Player") && gameManager.GetComponent<GameManger>().win)
            {
                SceneManager.LoadScene("Win");
               
            }
        }
    }
}
