using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody rb;

    public Material gasStationMat;
    public Material gasStationMat1;

    public Renderer gasGroundRenderer;

    [SerializeField] private CenterConnection centerConnection;

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
            if (gasGroundRenderer != null)
                    gasGroundRenderer.material = gasStationMat1;

            Debug.Log("Gas Triggered " + centerConnection.Distance.ToString("F2") + " cm");

             if (centerConnection != null)
            {
                centerConnection.SetActive(true);   
            }
            else
                Debug.Log("Gas Triggered fuera de 30cm");
            
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
