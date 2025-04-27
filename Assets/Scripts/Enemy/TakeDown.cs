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
    public GameObject catchCollider;
    public GameObject backCollider;

    public GameObject gem1;
    public string named;
    public EnemyBehavior enemyBehavior;


    int gemCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindWithTag("UI").GetComponent<InventoryManager>();
       eAgent = parent.GetComponent<NavMeshAgent>();
        named = parent.name;
        enemyBehavior = parent.GetComponent<EnemyBehavior>();



    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("The name of the parent is: " + named);
        switch (named) // depending on the neame 
        {
            case "FuryE":

                if (dead == true && inventory.takeDownItemCounterE >=1)
                {
                    inventory.takeDownItemCounterE -= 1;
                   
                    inventory.ShowAmount(inventory.takeDownItemTextE, inventory.takeDownItemCounterE, ref inventory.hasE);
                    StartCoroutine(furyTakeDown(2.5f));
                  

                }
                break;  

            case "FuryA":
                if (dead == true && inventory.takeDownItemCounterA >= 1)
                {
                    inventory.takeDownItemCounterA -= 1;
                    inventory.ShowAmount(inventory.takeDownItemTextA, inventory.takeDownItemCounterA, ref inventory.hasA);
                    StartCoroutine(furyTakeDown(2.5f));

                }
                break; 

            case "FuryT":

                if (dead == true && inventory.takeDownItemCounterT >= 1)
                {
                    inventory.takeDownItemCounterT -= 1;
                    inventory.ShowAmount(inventory.takeDownItemTextT, inventory.takeDownItemCounterT,ref inventory.hasT);
                    StartCoroutine(furyTakeDown(2f));
                }
                break;

        

            default:
                break;

        }

       // Debug.Log("the tag " + gameObject.tag);
        switch (parent.tag) // deoending on the tag
        {
            case "EnemyE":

                if (dead == true && inventory.takeDownItemCounterE >= 1)
                {
                    Debug.Log("how much is in it " + inventory.takeDownItemCounterE);

                    eAgent.isStopped = true;
                    Destroy(catchCollider);
                    eAgent.enabled = false;
                    enemyBehavior.enabled = false;
                    Debug.Log("Destroy colliders leave me alone");
                    Destroy(backCollider);

                    //backCollider.SetActive(false);
                }
                break;

            case "EnemyA":

                if (dead == true && inventory.takeDownItemCounterA >= 1)
                {
                    eAgent.isStopped = true;
                   
                    Destroy(catchCollider);
                    eAgent.enabled = false;
                    enemyBehavior.enabled = false;
                    Destroy(backCollider);

                }
                break;

            case "EnemyT": // tartarus 

                if (dead == true && inventory.takeDownItemCounterT >= 1)
                {
                    eAgent.isStopped = true;
                    Destroy(catchCollider);
                    eAgent.enabled = false;
                    enemyBehavior.enabled = false;
                    Destroy(backCollider);

                }
                break;


            default:
                break;
        }

    }

    IEnumerator furyTakeDown(float waitTime) // stop fury for waitTime seconds 
    {
        eAgent.isStopped = true;
        catchCollider.SetActive(false);
        //backCollider.SetActive(false);

        if (gemCount < 1)
        {
            Instantiate(gem1, dropItemPoint.transform.position, Quaternion.identity);
            gemCount++;
        }
        yield return new WaitForSeconds(waitTime);
        dead = false;   
        eAgent.isStopped = false;
        catchCollider.SetActive(true);
        //backCollider.SetActive(true); // can be removed depending if we want it to be able to be taken down again and again
    }

  
}
