using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TDO Obj", menuName = "Inventory System/Items/TDO")]

public class TakeDownObject : ItemObject
{
    public bool takeDownFury1;
    public bool takeDownFury2;
    public bool takeDownFury3;

    // Start is called before the first frame update
    void Awake()
    {
        type = ItemType.TakeDownItem;
        takeDownFury1 = false;
        takeDownFury2 = false;
        takeDownFury3 = false;
    }
}
