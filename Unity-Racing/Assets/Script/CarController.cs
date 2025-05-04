using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody rb;

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
        }
    }
}
