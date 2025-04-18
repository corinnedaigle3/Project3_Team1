using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionLight : MonoBehaviour
{
    [Header("Light Settings")]
    public Light Light;
    public bool LightDisabled;

    [Header("Waters Settings")]
    public Material Material_Original;
    public Material Material_Changed;
    public GameObject Object;


    private void Start()
    {
        RenderSettings.fogColor = new Color(1f, 0.7450980392156863f, 0.35294117647058826f);
        RenderSettings.fog = enabled;
        RenderSettings.fogDensity = 0.005f;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == other.CompareTag("Player"))
        {
            LightDisabled = true;
            Light.intensity = 0;

            Object.GetComponent<MeshRenderer>().material = Material_Changed;

            RenderSettings.fogColor = new Color(1f, 0f, 0f);
            RenderSettings.fogDensity = 0.001f;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == other.CompareTag("Player"))
        {
            LightDisabled = false;
            Light.intensity = 1;
            Object.GetComponent<MeshRenderer>().material = Material_Original;


            RenderSettings.fogColor = new Color(1f, 0.7450980392156863f, 0.35294117647058826f);
            RenderSettings.fogDensity = 0.005f;

        }
    }
}
