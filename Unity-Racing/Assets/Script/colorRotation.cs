using UnityEngine;

public class ColorRotationTexture : MonoBehaviour
{
    private Renderer carRenderer;
    private Renderer colorRenderer;
    private bool carReady = false;

    void Update()
    {
        // Si aún no se encontró el renderer, intenta buscarlo
        if (!carReady)
        {
            Transform bodyTransform = GameObject.Find("MyCar")?.transform.Find("Body");
            Transform colorTransform = GameObject.Find("Color Car")?.transform.Find("Body");

            if (bodyTransform != null && bodyTransform.gameObject.activeInHierarchy && colorTransform != null && colorTransform.gameObject.activeInHierarchy)
            {
                carRenderer = bodyTransform.GetComponent<Renderer>();
                colorRenderer = colorTransform.GetComponent<Renderer>();
                if (carRenderer != null && colorRenderer != null)
                {
                    carReady = true;
                    Debug.Log("Car renderer found and ready.");
                }
                else
                {
                    Debug.LogWarning("Body found, but no Renderer component.");
                }
            }
            else
            {
                Debug.Log("Waiting for MyCar/Body to appear and be active...");
                return;
            }
        }

        // Si ya está listo, aplicar la textura según rotación
        float angleY = transform.localEulerAngles.y;
        int snappedAngle = Mathf.RoundToInt(angleY / 90f) * 90 % 360;

        string textureName = snappedAngle switch
        {
            0 => "AFRC_Tex_Col1",
            90 => "AFRC_Tex_Col2",
            180 => "AFRC_Tex_Col3",
            270 => "AFRC_Tex_Col5",
            _ => null
        };

        if (!string.IsNullOrEmpty(textureName))
        {
            Texture texture = Resources.Load<Texture>(textureName);
            if (texture != null)
            {
                carRenderer.material.mainTexture = texture;
                colorRenderer.material.mainTexture = texture;
            }
            else
            {
                Debug.LogWarning($"Texture '{textureName}' not found in Resources.");
            }
        }
    }
}
