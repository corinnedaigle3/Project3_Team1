using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string writeNameOfScene;
    public GameObject gameManager;


    private void Awake()
    {
        gameManager = GameObject.Find("GameManager");
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
