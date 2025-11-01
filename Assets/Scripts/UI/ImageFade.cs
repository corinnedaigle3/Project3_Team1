using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    // Reference to the UI Image component that will fade in/out
    public Image image;

    // How long the fade in/out should take
    public float fadeDuration = 1f;

    // Reference to the PlayerMovement script to check player state
    PlayerMovement playerMovement;

    // Start is called before the first frame update
    private void Start()
    {
        // Find the player GameObject using its tag and get the PlayerMovement component
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        // If the player is dodging, start the fade-in coroutine
        if (playerMovement.isDodging == true)
        {
            StartCoroutine(FadeIn());
        }

        // If the player is NOT dodging, start the fade-out coroutine
        if (playerMovement.isDodging == false)
        {
            StartCoroutine(FadeOut());
        }

        if (playerMovement.canDodge == false)
        {
            SetInvisible();
        }
    }

    // Coroutine to gradually fade the image in (increase alpha)
    IEnumerator FadeIn()
    {
        float timeIn = 0; // Timer for the fade process
        Color startColor = image.color; // Starting color of the image
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Target color (fully visible)

        // Loop until fadeDuration is reached
        while (timeIn < fadeDuration)
        {
            // Interpolate alpha value over time
            float alpha = Mathf.Lerp(0, 1, timeIn / fadeDuration);
            image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            // Increase timer based on frame time
            timeIn += Time.deltaTime;

            // Wait until next frame before continuing
            yield return null;
        }

        // Ensure the final color is fully visible
        image.color = endColor;
    }

    private void SetInvisible()
    {
        Color color = image.color;
        color.a = 0f;
        image.color = color;
    }

    // Coroutine to gradually fade the image out (decrease alpha)
    IEnumerator FadeOut()
    {
        float timeOut = 0; // Timer for fade out
        Color startColor = image.color; // Starting color (likely visible)
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Target color (transparent)

        // Continue until alpha reaches 0
        while (image.color.a > 0)
        {
            // Increase timer and interpolate between start and end colors
            timeOut += Time.deltaTime;
            image.color = Color.Lerp(startColor, endColor, timeOut / fadeDuration);

            // Wait until next frame
            yield return null;
        }
    }
}
