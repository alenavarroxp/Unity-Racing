using UnityEngine;

public class GravityAR : MonoBehaviour
{
    public Transform groundPlane;
    public Light sceneLight;

    void Update()
    {
        if (groundPlane != null)
        {
            Physics.gravity = groundPlane.up * -9.81f;
        }

        if (sceneLight != null && groundPlane != null)
        {
            sceneLight.transform.rotation = groundPlane.rotation;
        }
    }
}
