using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class AIFollower : MonoBehaviour
{
    public Transform targetA;
    public Transform targetB;

    public bool shouldFollow = false;

    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!shouldFollow)
        {
            agent.isStopped = true;
            animator.SetFloat("MoveSpeed", 0f);
            return;
        }

        Transform activeTarget = GetActiveTarget();

        if (activeTarget != null)
        {
            agent.isStopped = false;
            agent.destination = activeTarget.position;

            // Actualizar animación según velocidad
            float speed = agent.velocity.magnitude;
            animator.SetFloat("MoveSpeed", speed);
        }
        else
        {
            agent.isStopped = true;
            animator.SetFloat("MoveSpeed", 0f);
        }
    }

    Transform GetActiveTarget()
    {
        if (targetA != null && targetA.gameObject.activeInHierarchy)
            return targetA;

        if (targetB != null && targetB.gameObject.activeInHierarchy)
            return targetB;

        return null;
    }

    public void SetShouldFollow(bool follow)
    {
        shouldFollow = follow;
    }
}
