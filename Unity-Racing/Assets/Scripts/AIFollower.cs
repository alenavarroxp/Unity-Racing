using UnityEngine;
using UnityEngine.AI;

public class AIFollower : MonoBehaviour
{
    public Transform target;
    public bool shouldFollow = false;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {   if(!shouldFollow) return;
    
        if (target != null)
        {
            agent.destination = target.position;
        }
    }

    public void SetShouldFollow(bool follow)
    {
        shouldFollow = follow;
    }
}
