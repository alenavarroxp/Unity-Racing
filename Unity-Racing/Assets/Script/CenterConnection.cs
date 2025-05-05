using UnityEngine;
using Ilumisoft.HealthSystem;

public class CenterConnection : MonoBehaviour
{
    [SerializeField] private GameObject gasCenter;
    [SerializeField] private GameObject chargerCenter; 
    [SerializeField] private Health health;

    private LineRenderer lineRenderer;

    private float distance;

    public float Distance { get { return distance; } }
    private bool isActive = false;

    public void SetActive(bool value)
    {
        isActive = value;

        if (!isActive && lineRenderer != null)
        {
            lineRenderer.enabled = false;
            distance = Mathf.Infinity;
        }
    }

    void Update()
    {
        if (!isActive) return;

        if (gasCenter.activeSelf && chargerCenter.activeSelf)
        {
         if (lineRenderer == null)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.startWidth = 0.01f;
                lineRenderer.endWidth = 0.01f;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = Color.black;
                lineRenderer.endColor = Color.black;
                lineRenderer.positionCount = 2;
            }

            lineRenderer.enabled = true;

            Vector3 gasPos = gasCenter.transform.position;
            Vector3 carPos = chargerCenter.transform.position;

            lineRenderer.SetPosition(0, gasPos);
            lineRenderer.SetPosition(1, carPos);

            distance = Vector3.Distance(gasPos, carPos);

            if(distance < 0.3f)
            {
                if (health != null)
                {
                    health.RefillHealth();
                    Debug.Log("Gas Triggered " + distance.ToString("F2") + " cm");
                }
            }
           
        }
        else
        {
            if (lineRenderer != null)
            {
                lineRenderer.enabled = false;
                distance = Mathf.Infinity;
            }
        }
    }

}
