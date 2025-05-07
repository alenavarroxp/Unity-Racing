using UnityEngine;
using System.Collections;

public class SafeCarActivator : MonoBehaviour
{
    public GameObject carObject;
    private bool isCarActive = false;

    public void PlaceCar()
    {  
        if (isCarActive) return; // Evitar activar el coche si ya está activo
        StartCoroutine(ActivateCarSafely());
    }

    private IEnumerator ActivateCarSafely()
    {
        Rigidbody rb = carObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        yield return null;

        isCarActive = true; // Marcar el coche como activo
        carObject.SetActive(true); // Solo activarlo, sin tocar su posición

        if (rb != null) rb.isKinematic = false;
    }
}
