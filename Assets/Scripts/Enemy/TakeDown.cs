using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class TakeDown : MonoBehaviour
{
    public InventoryManager inventory;
    public GameObject parent;
    public NavMeshAgent eAgent;
    public bool dead = false;
    public Transform dropItemPoint;
    public Collider c;

    public GameObject gem1;
    public string named;


    int gemCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindWithTag("UI").GetComponent<InventoryManager>();
       eAgent = parent.GetComponent<NavMeshAgent>();
        named = parent.name;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (named) // depending on the neame 
        {
            case "FuryE":

                if (dead == true && inventory.TakeDownItemEcounter >=1)
                {
                   StartCoroutine(furyTakeDown(1f));

                }
                break;  

            case "FuryA":
                if (dead == true && inventory.TakeDownItemAcounter >= 1)
                {
                    StartCoroutine(furyTakeDown(1f));

                }
                break; 

            case "FuryT":

                if (dead == true && inventory.TakeDownItemTcounter >= 1)
                {
                    StartCoroutine(furyTakeDown(1f));
                }
                break;

        

            default:
                break;

        }

       // Debug.Log("the tag " + gameObject.tag);
        switch (parent.tag) // deoending on the tag
        {
            case "EnemyE":

                Debug.Log( "how much is in it " + inventory.TakeDownItemEcounter);
                if (dead == true && inventory.TakeDownItemEcounter >= 1)
                {
                    eAgent.isStopped = true;
                    c.enabled = false;
                }
                break;

            case "EnemyA":

                if (dead == true && inventory.TakeDownItemAcounter >= 1)
                {
                    eAgent.isStopped = true;
                    c.enabled = false;
                }
                break;

            case "EnemyT": // tartarus 

                if (dead == true && inventory.TakeDownItemTcounter >= 1)
                {
                    eAgent.isStopped = true;
                    c.enabled = false;
                }
                break;


            default:
                break;
        }

    }

    IEnumerator furyTakeDown(float waitTime) // stop fury for waitTime seconds 
    {
        eAgent.isStopped = true;
        c.enabled = false;
        if (gemCount < 1)
        {
            Instantiate(gem1, dropItemPoint.transform.position, Quaternion.identity);
            gemCount++;
        }
        yield return new WaitForSeconds(waitTime);
        eAgent.isStopped = false;
        c.enabled = true;
    }

  
}
