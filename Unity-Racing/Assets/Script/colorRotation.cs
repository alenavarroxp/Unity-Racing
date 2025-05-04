using UnityEngine;

public class ColorRotationTexture : MonoBehaviour
{
    private Renderer carRenderer;
    private Renderer colorRenderer;
    private bool carReady = false;
    private bool isActive = true;

    public void SetActive(bool value)
    {
        isActive = value;
    }

    void Update()
    {
        if (!isActive) return;

        if (!carReady)
        {
            Transform bodyTransform = GameObject.Find("MyCar")?.transform.Find("Body");
            Transform colorTransform = GameObject.Find("Color Car")?.transform.Find("Body");

            if (bodyTransform != null && bodyTransform.gameObject.activeInHierarchy &&
                colorTransform != null && colorTransform.gameObject.activeInHierarchy)
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

        float angleY = transform.localEulerAngles.y % 360f;
        string textureName = null;

        if (angleY >= 0f && angleY < 90f)
            textureName = "AFRC_Tex_Col1";
        else if (angleY >= 90f && angleY < 180f)
            textureName = "AFRC_Tex_Col2";
        else if (angleY >= 180f && angleY < 270f)
            textureName = "AFRC_Tex_Col3";
        else if (angleY >= 270f && angleY < 360f)
            textureName = "AFRC_Tex_Col5";

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
