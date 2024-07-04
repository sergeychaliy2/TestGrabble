using System.Collections;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public Color targetColor = Color.black;
    public float colorChangeSpeed = 0.1f;
    private Renderer objectRenderer;
    private Color currentColor;
    private bool isChangingColor = false;
    public Light flashLight;
    private Color bfColor = new Color(191f / 255f, 191f / 255f, 191f / 255f);
    private float colorThreshold = 0.1f;
    private bool isFullyBlack = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        currentColor = objectRenderer.material.color;

        if (flashLight != null)
        {
            flashLight.enabled = false;
        }
    }

    public void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Water"))
        {
            if (!isChangingColor && !isFullyBlack)
            {
                StartCoroutine(ChangeColorOverTime(colorChangeSpeed));
            }
            if (!isFullyBlack)
            {
                StartCoroutine(FlashLight());
            }
        }
    }

    private IEnumerator FlashLight()
    {
        if (flashLight != null)
        {
            flashLight.enabled = true;
            yield return new WaitForSeconds(0.1f);
            flashLight.enabled = false;
        }
    }

    private IEnumerator ChangeColorOverTime(float duration)
    {
        isChangingColor = true;

        Color startColor = currentColor;
        Color newTargetColor = Color.Lerp(startColor, targetColor, colorChangeSpeed);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            currentColor = Color.Lerp(startColor, newTargetColor, elapsedTime / duration);
            objectRenderer.material.color = currentColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentColor = newTargetColor;
        objectRenderer.material.color = currentColor;
        isChangingColor = false;

        if (IsColorCloseToBlack(currentColor))
        {
            isFullyBlack = true;
            if (flashLight != null)
            {
                flashLight.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            StopAllCoroutines();
            isChangingColor = false;
            if (flashLight != null && !isFullyBlack)
            {
                flashLight.enabled = false;
            }
        }
    }

    private bool IsColorCloseToBlack(Color color)
    {
        return color.r <= colorThreshold && color.g <= colorThreshold && color.b <= colorThreshold;
    }
}
