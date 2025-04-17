using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    public Image image;
    public float fadeDuration = 1f;
    PlayerMovement playerMovement;

    // Start is called before the first frame update
    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (playerMovement.isDodging == true)
        {
            StartCoroutine(FadeOut());
        }
        if (playerMovement.isDodging == false)
        {
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        float timeIn = 0;
        Color startColor = image.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Target alpha is 1

        while (timeIn < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, timeIn / fadeDuration);
            image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            timeIn += Time.deltaTime;
            yield return null;
        }

        image.color = endColor; // Ensure we end with fully opaque
    }

    IEnumerator FadeOut()
    {
        float timeOut = 0;
        Color startColor = image.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (image.color.a > 0)
        {
            timeOut += Time.deltaTime;
            image.color = Color.Lerp(startColor, endColor, timeOut / fadeDuration);
            yield return null;
        }
    }
}
