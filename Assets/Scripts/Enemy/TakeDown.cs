using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TakeDown : MonoBehaviour
{
    public GameObject parent;
    public NavMeshAgent eAgent;
    public bool dead;
    // Start is called before the first frame update
    void Start()
    {
        
       eAgent = parent.GetComponent<NavMeshAgent>();
      
    }

    // Update is called once per frame
    void Update()
    {
     
        if (dead)
        {
            eAgent.isStopped = true;
            gameObject.SetActive(false);
        }
    }
}
