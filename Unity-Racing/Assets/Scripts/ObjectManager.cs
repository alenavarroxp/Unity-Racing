using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject[] collectiblePrefabs;
    public Transform[] spawnPoints;

    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int index = Random.Range(0, collectiblePrefabs.Length);
            Instantiate(collectiblePrefabs[index], spawnPoints[i].position, Quaternion.identity);
        }
    }
}
