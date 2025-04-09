using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavableObjects : MonoBehaviour
{
    public string objectId; // Set this in inspector or generate automatically

    void Awake()
    {
        if (string.IsNullOrEmpty(objectId))
        {
            objectId = $"{SceneManager.GetActiveScene().name}_{gameObject.name}_{transform.position}";
        }
    }
}
