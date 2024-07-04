using UnityEngine;

public class SphereController : MonoBehaviour
{
    public Color targetColor = Color.green;
    public float colorChangeSpeed = 0.1f;
    public float scaleChangeSpeed = 0.1f;
    public float minScale = 0.1f;
    private Renderer objectRenderer;
    private Color initialColor;
    private Vector3 initialScale;
    private bool isChangingColor = false;
    public ParticleSystem smokeParticles;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        initialColor = objectRenderer.material.color;
        initialScale = transform.localScale;
        if (smokeParticles != null)
        {
            smokeParticles.Stop();
        }
    }

    void Update()
    {
        if (isChangingColor)
        {
            objectRenderer.material.color = Color.Lerp(objectRenderer.material.color, targetColor, colorChangeSpeed * Time.deltaTime);
            float newScale = Mathf.Lerp(transform.localScale.x, minScale, scaleChangeSpeed * Time.deltaTime);
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }

    public void OnParticleCollision (GameObject other)
    {
        if (other.CompareTag("Water"))
        {
            isChangingColor = true;
            PlaySmokeEffect();
        }
    }

    private void PlaySmokeEffect()
    {
        if (smokeParticles != null)
        {
            smokeParticles.transform.position = transform.position;
            smokeParticles.Play(); 
        }
    }
}
