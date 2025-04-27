using UnityEngine;
using System.Collections;

public class SafeCarActivator : MonoBehaviour
{
    public GameObject carObject;

    public void PlaceCar()
    {
        StartCoroutine(ActivateCarSafely());
    }

    private IEnumerator ActivateCarSafely()
    {
        carObject.SetActive(true);

        Rigidbody rb = carObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        else
        {
            Debug.LogError("No se encontró Rigidbody en el coche.");
        }

        yield return null;

        carObject.transform.localPosition = new Vector3(0f, 0.2f, 0f);

        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}
