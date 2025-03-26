using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Only gamplay")]
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;
}

public enum ItemType
{
    Elysium,
    Asphodel,
    Tartarus,
    Helm,
    Fury1Gem,
    Fury2Gem,
    Fury3Gem
}

public enum ActionType
{
    Invisiblity,
    TakeDown,
    Teleport
}