using UnityEngine;
using Vuforia;

public class PlacePrefab : MonoBehaviour
{
    public GameObject prefab;

    private GameObject instance;

    public void PlaceAt(HitTestResult result)
    {
        if (instance == null)
        {
            instance = Instantiate(prefab, result.Position, result.Rotation);
        }
    }
}
