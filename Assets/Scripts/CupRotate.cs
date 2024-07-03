using UnityEngine;

public class CupRotate : MonoBehaviour
{
    public WaterController waterDropsController;

    void Update()
    {
        if (transform.eulerAngles.z > 45 && transform.eulerAngles.z < 315)
        {
            waterDropsController.Pour(true);
        }
        else
        {
            waterDropsController.Pour(false);
        }
    }
}
