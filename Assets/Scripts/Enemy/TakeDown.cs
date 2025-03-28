using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TakeDown : MonoBehaviour
{
    public GameObject parent;
    public NavMeshAgent eAgent;
    public bool dead;
    [HideInInspector] public GameObject dropItemInstance;
    public  Transform dropItemPoint;

    [SerializeField] private GameObject fury1Gem;

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
            dropItemInstance = Instantiate(fury1Gem, dropItemPoint.transform.position, dropItemPoint.rotation);
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
