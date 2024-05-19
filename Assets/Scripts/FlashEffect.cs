using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    public Material flashMaterial;
    public SpriteRenderer spriteRenderer;
    Material originalMaterial;
    Color originalColor;
    private void Start()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("FlashEffect: No SpriteRenderer found on GameObject");
        }
        originalMaterial = spriteRenderer.material;
        originalColor = spriteRenderer.color;
    }
    public void Flash(Color color, float duration)
    {
        StartCoroutine(FlashCoroutine(color, duration));
    }

    public void FlashBlink(Color color, float duration, int blinkCount, float blinkRatio)
    {
        StartCoroutine(FlashBlinkCoroutine(color, duration, blinkCount, blinkRatio));
    }

    private IEnumerator FlashCoroutine(Color color, float duration)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("FlashEffect: No SpriteRenderer found on GameObject");
            yield break;
        }

        
        spriteRenderer.material = flashMaterial;
        spriteRenderer.color = color;
        yield return new WaitForSeconds(duration);

        spriteRenderer.material = originalMaterial;
        spriteRenderer.color = originalColor;
    }

    private IEnumerator FlashBlinkCoroutine(Color color, float duration, int blinkCount, float blinkRatio) // e.g. 1f, 2, 0.5f
    {
        // blinkRatio is the ratio of time spent blinking to time spent not blinking
        if (spriteRenderer == null)
        {
            Debug.LogError("FlashEffect: No SpriteRenderer found on GameObject");
            yield break;
        }


        yield return new();
        for (int i = 0; i < blinkCount; i++)
        {
            yield return new();
            spriteRenderer.material = flashMaterial;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(duration * blinkRatio / (blinkCount*2)); // flash duration; 0.5/4 = 0.125
            
            spriteRenderer.material = originalMaterial;
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(duration * (1 - blinkRatio) / (blinkCount*2)); // wait duration; 1.5/4 = 0.375
        }

        spriteRenderer.material = originalMaterial;
        spriteRenderer.color = originalColor;
    }
}
