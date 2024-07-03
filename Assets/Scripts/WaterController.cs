using UnityEngine;

public class WaterController : MonoBehaviour
{
    public ParticleSystem waterDrops;
    private ParticleSystem.EmissionModule emissionModule;
    private bool isPouring = false;

    void Start()
    {
        emissionModule = waterDrops.emission;
        emissionModule.rateOverTime = 0;
    }

    void Update()
    {
        if (isPouring)
        {
            emissionModule.rateOverTime = 2;
        }
        else
        {
            emissionModule.rateOverTime = 0;
        }
    }

    public void Pour(bool pour)
    {
        isPouring = pour;
    }
}
