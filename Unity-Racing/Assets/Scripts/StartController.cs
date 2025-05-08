using UnityEngine;

public class StartController : MonoBehaviour
{
    public GameObject objectManager;
    public GameObject lampostManager;
    public GameObject fuelBar;
    public GameObject turboBar;
    public GameObject objectBar;
    public GameObject buttons;
    public GameObject speedText;

    public void ActivateManagers()
    {
        if (objectManager != null) objectManager.SetActive(true);
        if (lampostManager != null) lampostManager.SetActive(true);
        if (fuelBar != null) fuelBar.SetActive(true);
        if (turboBar != null) turboBar.SetActive(true);
        if (objectBar != null) objectBar.SetActive(true);
        if (buttons != null) buttons.SetActive(true);
        if (speedText != null) speedText.SetActive(true);
    }
}
