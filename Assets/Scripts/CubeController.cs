using System.Collections;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public Color targetColor = Color.black;
    public float colorChangeSpeed = 0.5f;
    public float flashInterval = 0.5f;
    private Renderer objectRenderer;
    private Color initialColor;
    private bool isChangingColor = false;
    private bool isBlack = false;
    public Light flashLight;
    public GameObject explosionPrefab;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        initialColor = objectRenderer.material.color;

        if (flashLight != null)
        {
            flashLight.enabled = false;
        }
    }

    void Update()
    {
        if (isChangingColor)
        {
            objectRenderer.material.color = Color.Lerp(objectRenderer.material.color, targetColor, colorChangeSpeed * Time.deltaTime);
            if (!isBlack && objectRenderer.material.color.Equals(targetColor))
            {
                isBlack = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isChangingColor = true;
            StartCoroutine(FlashCoroutine());
        }
    }

    private IEnumerator FlashCoroutine()
    {
        while (!isBlack && isChangingColor)
        {
            if (flashLight != null)
            {
                flashLight.enabled = !flashLight.enabled;
            }
            yield return new WaitForSeconds(flashInterval);
        }

        if (flashLight != null)
        {
            flashLight.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (objectRenderer.material.color == targetColor)
        {
            if (collision.gameObject.CompareTag("Cylinder"))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
