using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class TakeDown : MonoBehaviour
{
    public GameObject parent;
    public NavMeshAgent eAgent;
    public bool dead = false;
    public  Transform dropItemPoint;

    public GameObject fury1Gem;
    public string name;

    // Start is called before the first frame update
    void Start()
    {
       eAgent = parent.GetComponent<NavMeshAgent>();
        name = parent.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true && name == "Enemy")
        {
            eAgent.isStopped = true;
            //gameObject.SetActive(false);
        } else if (dead == true)
        {
            StartCoroutine(furyTakeDown(3f));
        }
        else if(dead == false)
        {
            eAgent.isStopped = false;
            gameObject.SetActive(true);
        }
    }

    IEnumerator furyTakeDown(float waitTime) // stop fury for waitTime seconds 
    {
        eAgent.isStopped = true;
        Instantiate(fury1Gem, dropItemPoint.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(waitTime);
        eAgent.isStopped = false;
    }

    private void Drop()
    {
        //remove the iten from inventory
    }
}
