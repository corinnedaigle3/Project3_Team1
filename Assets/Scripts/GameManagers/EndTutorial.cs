using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTutorial : MonoBehaviour
{
    public InventoryManager i;
    GameManger manager;
    // Start is called before the first frame update
    void Start()
    {
        if (i == null)
        i = GameObject.FindWithTag("UI").GetComponent<InventoryManager>();

        if (manager == null)
        manager = GameObject.Find("GameManager").GetComponent<GameManger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.tutorialDone = true;
            if (i.takeDownItemCounterE > 0)
            {
                i.takeDownItemCounterE -= 1;
            i.ShowAmount(i.takeDownItemTextE, i.takeDownItemCounterE, ref i.hasE);
            }
            if (i.takeDownItemCounterA > 0)
            {
                i.takeDownItemCounterA -= 1;
                i.ShowAmount(i.takeDownItemTextA, i.takeDownItemCounterA, ref i.hasA);
            }
            if (i.takeDownItemCounterT > 0)
            {
                i.takeDownItemCounterT -= 1;
                i.ShowAmount(i.takeDownItemTextT, i.takeDownItemCounterT, ref i.hasT);
            }
            Debug.Log("Beat The Tutorial");

        }
    }
}
