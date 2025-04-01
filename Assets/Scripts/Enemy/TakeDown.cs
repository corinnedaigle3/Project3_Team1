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

    public GameObject gem1;
    public string named;


    int gemCount = 0;

    // Start is called before the first frame update
    void Start()
    {
       eAgent = parent.GetComponent<NavMeshAgent>();
        named = parent.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true && named == "Enemy")
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
        if (gemCount < 1)
        {
            Instantiate(gem1, dropItemPoint.transform.position, Quaternion.identity);
            gemCount++;
        }
        yield return new WaitForSeconds(waitTime);
        eAgent.isStopped = false;
    }

    private void Drop()
    {
        //remove the iten from inventory
    }
}
