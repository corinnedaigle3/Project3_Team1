using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Only gamplay")]
    public TileBase tile;
    
    public ToolboxItemFilterType type;
    public ItemType itemType;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]

    [Header("Both")]
    public Sprite image;
}

public enum ItemType
{
    Elysium,
    Asphodel,
    Tartarus
}

