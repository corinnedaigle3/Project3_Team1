using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElysiumManager : MonoBehaviour
{
    public GameObject TakeDownItemE;
    public GameObject Helm;
    //public GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(TakeDownItemE, new Vector3(-97.12f, 3.99f, -128.56f), Quaternion.identity);
        Instantiate(TakeDownItemE, new Vector3(132.55f, 2.65f, -7.14f), Quaternion.identity);
        Instantiate(TakeDownItemE, new Vector3(156.02f, 8f, -129.34f), Quaternion.identity);
        Instantiate(TakeDownItemE, new Vector3(72.18f, 10.58f, 69.14f), Quaternion.identity);

        Instantiate(Helm, new Vector3(160.69f, 9.87f, -69.44f), Quaternion.identity);
        Instantiate(Helm, new Vector3(45.50719f, 4.5767f, -12.17f), Quaternion.identity);
        Instantiate(Helm, new Vector3(63.82953f, 10.98f, 80.2f), Quaternion.identity);
    }
        // Update is called once per frame
    void Update()
    {
        
    }
}
