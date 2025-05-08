using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject[] objects;

    public void EnableRigidBodies(bool enable)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                ObjectController controller = obj.GetComponent<ObjectController>();
                if (controller != null)
                {
                    controller.enableRigidBody(enable);
                }
            }
        }
    }
}
