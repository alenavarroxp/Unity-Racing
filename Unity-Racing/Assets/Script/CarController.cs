using UnityEngine;
using Ilumisoft.HealthSystem;

public class CarController : MonoBehaviour
{
    private Rigidbody rb;

    public Material gasStationMat;
    public Material gasStationMat1;

    public Renderer gasGroundRenderer;

    [SerializeField] private Health health;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void enableRigidBody(bool enable)
    {
        if (rb == null) 
            rb = GetComponent<Rigidbody>();

        if (enable)
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
        }
        else
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Algo entró al trigger: " + other.name);
        if (other.CompareTag("Gas"))
        {
            Debug.Log("Gas Triggered");
            if (gasGroundRenderer != null)
            {
                gasGroundRenderer.material = gasStationMat1;
                if (health != null)
                    health.RefillHealth();
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Algo salió del trigger: " + other.name);

        if (other.CompareTag("Gas"))
        {
            Debug.Log("Gas Exited");

            if (gasGroundRenderer != null)
            {
                gasGroundRenderer.material = gasStationMat;
            }
        }
    }
}
