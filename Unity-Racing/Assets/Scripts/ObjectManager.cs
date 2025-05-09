using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject[] objects;

    private int countObjects = 0;

    public int GetCountObjects()
    {
        return countObjects;
    }

    public void AddObject()
    {
        countObjects++;
        Debug.Log("Count Objects: " + countObjects);
    }

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
