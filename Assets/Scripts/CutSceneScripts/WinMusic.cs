using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMusic : MonoBehaviour
{
    public AudioSource background;
    private float musicTimer;
    private bool musicOn;

    // Start is called before the first frame update
    void Start()
    {
        musicTimer = 0f;
        musicOn = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        musicTimer += Time.deltaTime;

        if (musicTimer > 7f && !musicOn)
        {
            musicOn = true;
            background.Play();
        }
    }
}