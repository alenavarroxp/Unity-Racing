using UnityEngine;

public class ColorRotationTexture : MonoBehaviour
{
    [SerializeField] private GameObject colorCar;
    [SerializeField] private GameObject secondColorCar;

    private Renderer carRenderer;
    private Renderer colorRenderer;

    private GameObject currentActiveCar;
    private GameObject currentColorCar;

    private bool isActive = true;

    public void SetActive(bool value) => isActive = value;

    void Start()
    {
        colorCar?.SetActive(false);
        secondColorCar?.SetActive(false);
    }

    void Update()
    {
        if (!isActive) return;

        GameObject myCar = GameObject.Find("MyCar");
        GameObject secondCar = GameObject.Find("SecondCar");

        bool myCarActive = myCar != null && myCar.activeInHierarchy;
        bool secondCarActive = secondCar != null && secondCar.activeInHierarchy;

        Debug.Log("MyCar Active: " + myCarActive);
        Debug.Log("SecondCar Active: " + secondCarActive);

        GameObject newActiveCar = myCarActive ? myCar : (secondCarActive ? secondCar : null);
        GameObject newColorCar = myCarActive ? colorCar : (secondCarActive ? secondColorCar : null);

        // Si cambia el coche activo, actualiza renderers y colorCar visibles
        if (newActiveCar != currentActiveCar)
        {
            currentActiveCar = newActiveCar;
            currentColorCar = newColorCar;

            if (colorCar != null) colorCar.SetActive(colorCar == currentColorCar);
            if (secondColorCar != null) secondColorCar.SetActive(secondColorCar == currentColorCar);

            if (currentActiveCar != null)
            {
                carRenderer = currentActiveCar.transform.Find("Body")?.GetComponent<Renderer>();
                colorRenderer = currentColorCar?.transform.Find("Body")?.GetComponent<Renderer>();

                Debug.Log("Updated renderers for new active car.");
            }
            else
            {
                carRenderer = null;
                colorRenderer = null;
                Debug.Log("No active car found.");
            }
        }

        if (carRenderer == null || colorRenderer == null) return;

        float angleY = transform.localEulerAngles.y % 360f;
        string textureName = angleY switch
        {
            >= 0f and < 90f => "AFRC_Tex_Col1",
            >= 90f and < 180f => "AFRC_Tex_Col2",
            >= 180f and < 270f => "AFRC_Tex_Col3",
            >= 270f and < 360f => "AFRC_Tex_Col5",
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
