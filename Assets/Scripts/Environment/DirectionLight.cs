using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionLight : MonoBehaviour
{
    public Light Light;
    public bool LightDisabled;

    private void Start()
    {
        RenderSettings.fogColor = new Color(1f, 0.7450980392156863f, 0.35294117647058826f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == other.CompareTag("Player"))
        {
            LightDisabled = true;
            Light.intensity = 0;
            RenderSettings.fogColor = new Color(1f, 0f, 0f);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == other.CompareTag("Player"))
        {
            LightDisabled = false;
            Light.intensity = 1;
            RenderSettings.fogColor = new Color(1f, 0.7450980392156863f, 0.35294117647058826f);

        }
    }
}
