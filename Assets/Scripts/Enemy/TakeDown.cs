using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TakeDown : MonoBehaviour
{
    public GameObject parent;
    public NavMeshAgent eAgent;
    public bool dead;
    private GameObject dropItemInstance;
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
     
        if (dead)
        {
            eAgent.isStopped = true;
            dropItemInstance = Instantiate(fury1Gem, dropItemPoint.transform.position, dropItemPoint.rotation);
            gameObject.SetActive(false);
        }
    }


    private void Drop()
    {
        //remove the iten from inventory
    }
}
