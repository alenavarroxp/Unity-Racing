using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    public ParticleSystem collectEffect;
    public ParticleSystem areaEffect;
    private AudioSource audioSource;

    public ObjectManager objectManager;

    [SerializeField] public bool canRotate = true;

    void Awake()
    {
        var soundObj = GameObject.Find("Collectible Sound");
        if (soundObj != null) audioSource = soundObj.GetComponent<AudioSource>();
        else Debug.LogWarning("AudioSource 'Drill Sound' no encontrado.");
    }

    void Update()
    {
        if (canRotate)
        {
            transform.Rotate(0, 100 * Time.deltaTime, 0);
        }
    }

    public void Collect()
    {
    
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity).Play();
        }

        if(areaEffect != null)
        {
            Instantiate(areaEffect, transform.position, Quaternion.identity).Play();
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (objectManager != null)
        {
            objectManager.AddObject();
        }

        Destroy(gameObject);
    }

}
