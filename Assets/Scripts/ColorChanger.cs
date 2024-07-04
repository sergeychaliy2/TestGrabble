using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Color targetColor = Color.blue;
    public float colorChangeSpeed = 0.1f;
    private Renderer objectRenderer;
    private Color initialColor;
    private bool isChangingColor = false;
    public ParticleSystem sparks;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        initialColor = objectRenderer.material.color;
    }

    void Update()
    {
        if (isChangingColor)
        {
            objectRenderer.material.color = Color.Lerp(objectRenderer.material.color, targetColor, colorChangeSpeed * Time.deltaTime);
        }
    }

    public void OnParticleCollision (GameObject other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            isChangingColor = true;
            PlaySparksEffect();
            Debug.Log("Water");
        }
    }

    private void PlaySparksEffect()
    {
        if (sparks != null)
        {
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
            emitParams.position = transform.position;
            sparks.Emit(emitParams, 30);
        }
    }
}
