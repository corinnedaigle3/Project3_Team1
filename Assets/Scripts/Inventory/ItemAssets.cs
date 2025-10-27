using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    // gets and sets the sprites
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    //Sets-up / holds the sprite images
    public Sprite appleSprite;
    public Sprite skullSprite;
    public Sprite fireFlowerSprite;
    public Sprite gemSprite1;
    public Sprite gemSprite2;
    public Sprite gemSprite3;
    public Sprite helmSprite;
}   