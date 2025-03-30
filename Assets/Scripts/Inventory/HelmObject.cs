using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Helm Obj", menuName = "Inventory System/Items/Helm")]
public class HelmObject : ItemObject
{
    public bool invisiblityOn;

    void Awake()
    {
        type = ItemType.Helm;
        invisiblityOn = false;
    }
}
