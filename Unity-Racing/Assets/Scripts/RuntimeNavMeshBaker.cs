using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

[RequireComponent(typeof(NavMeshSurface))]
public class RuntimeNavMeshBaker : MonoBehaviour
{
    private NavMeshSurface surface;

    void Start()
    {
        surface = GetComponent<NavMeshSurface>();
    }

    public void RebuildNavMesh()
    {
        surface.BuildNavMesh();
    }

}
