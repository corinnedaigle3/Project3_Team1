using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElysiumManager : MonoBehaviour
{
    public GameObject TakeDownItemE;
    public GameObject Helm;
    public GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(TakeDownItemE, new Vector3(), Quaternion.identity);
        Instantiate(TakeDownItemE, new Vector3(), Quaternion.identity);
        Instantiate(TakeDownItemE, new Vector3(), Quaternion.identity);

        Instantiate(Helm, new Vector3(), Quaternion.identity);
        Instantiate(Helm, new Vector3(), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
