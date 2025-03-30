using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TakeDown : MonoBehaviour
{
    public GameObject parent;
    public NavMeshAgent eAgent;
    public bool dead;
    public  Transform dropItemPoint;

    public GameObject fury1Gem;

    // Start is called before the first frame update
    void Start()
    {
       eAgent = parent.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true)
        {
            eAgent.isStopped = true;
            Instantiate(fury1Gem, dropItemPoint.transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
        else if(dead == false)
        {
            eAgent.isStopped = false;
            gameObject.SetActive(true);
        }
    }


    private void Drop()
    {
        //remove the iten from inventory
    }
}
