using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gem Obj", menuName = "Inventory System/Items/Gem")]
public class GemObject : ItemObject
{
    public bool teleportFury1;
    public bool teleportFury2;
    public bool teleportFury3;

    void Awake()
    {
        type = ItemType.Gem;
        teleportFury1 = false;
        teleportFury2 = false;
        teleportFury3 = false;
    }
}
