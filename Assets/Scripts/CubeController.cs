using System.Collections;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public Color targetColor = new Color(191f / 255f, 191f / 255f, 191f / 255f);
    public float colorChangeSpeed = 0.1f;
    private Renderer objectRenderer;
    private Color currentColor;
    private bool isChangingColor = false;
    public Light flashLight;
    private float colorThreshold = 0.1f;
    private bool isFullyBlack = false;
    public SphereController sphere;

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

            if (sphere != null && IsColorCloseToBlack(currentColor))
            {
                Collider cubeCollider = GetComponent<Collider>();
                Collider sphereCollider = sphere.GetComponent<Collider>();
                if (cubeCollider != null && sphereCollider != null && cubeCollider.bounds.Intersects(sphereCollider.bounds))
                {
                    if (currentColor.r >= targetColor.r - colorThreshold &&
                        currentColor.g >= targetColor.g - colorThreshold &&
                        currentColor.b >= targetColor.b - colorThreshold)
                    {
                        sphere.gameObject.SetActive(false);
                    }
                }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sphere"))
        {
            if (IsColorCloseToBlack(currentColor))
            {
                sphere.gameObject.SetActive(false);
            }
        }
    }
}
