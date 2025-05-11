using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class CarController : MonoBehaviour
{
    private Rigidbody rb;

    public Material gasStationMat;
    public Material gasStationMat1;

    public Renderer gasGroundRenderer;

    [SerializeField] private CenterConnection centerConnection;
    [SerializeField] private ObjectBar objectBar;

    private List<string> logLines = new List<string>();
    private string filePath;
    private bool isLogging = false;
    private bool collectedThisFrame = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Definir la ruta donde se guardará el CSV
        string resultsDir = Path.Combine(Application.dataPath, "Results");
        if (!Directory.Exists(resultsDir))
            Directory.CreateDirectory(resultsDir);

        filePath = Path.Combine(resultsDir, "car_positions.csv");
        Debug.Log("CSV guardado en: " + filePath);

        // Cabecera del CSV, se usa ; como delimitador
        logLines.Add("Time;X;Y;Z;Collected");

        isLogging = true;
    }

    void Update()
    {
        if (!isLogging) return;

        Vector3 pos = transform.position;

        // Registrar la línea con ; como delimitador
        string line = $"{Time.time:F2};{pos.x:F4};{pos.y:F4};{pos.z:F4};{collectedThisFrame}";
        logLines.Add(line);

        collectedThisFrame = false;
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

            GameObject FuelPump = GameObject.Find("FuelPump");

            if (FuelPump != null)
                FuelPump.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f);

            Debug.Log("Gas Triggered " + centerConnection.Distance.ToString("F2") + " cm");

            if (centerConnection != null)
                centerConnection.SetActive(true);
        }

        if (other.CompareTag("Collectible"))
        {
            other.GetComponent<CollectibleObject>().Collect();
            collectedThisFrame = true;

            if (objectBar != null)
                objectBar.AddObject();
        }

        if (other.CompareTag("Deadpool"))
        {
            GameObject crown = GameObject.Find("DeadpoolCrownObject");
            if (crown != null)
            {
                CollectibleObject collectible = crown.GetComponent<CollectibleObject>();
                if (collectible != null)
                {
                    collectible.Collect();
                    collectedThisFrame = true;
                }

                Destroy(crown);

                if (objectBar != null)
                    objectBar.AddObject();
            }

            Debug.Log("Deadpool ModelTarget recogido.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Algo salió del trigger: " + other.name);

        if (other.CompareTag("Gas"))
        {
            Debug.Log("Gas Exited");

            if (gasGroundRenderer != null)
                gasGroundRenderer.material = gasStationMat;

            GameObject FuelPump = GameObject.Find("FuelPump");
            if (FuelPump != null)
                FuelPump.transform.localScale = new Vector3(0.006f, 0.006f, 0.006f);
        }
    }

    public void StopLogging()
    {
        isLogging = false;

        File.WriteAllText(filePath, string.Join("\n", logLines), Encoding.UTF8);

        Debug.Log("CSV guardado en: " + filePath);
    }

    public void AddFinalLogEntry(int lastObjectNumber, float lastObjectTime)
    {
        // Usamos la última posición conocida del coche
        Vector3 carPosition = transform.position;

        // Crear la línea final con los valores correctos
        string finalLine = $"{lastObjectTime:F2};{carPosition.x:F4};{carPosition.y:F4};{carPosition.z:F4};True";

        // Añadir la última línea al archivo CSV
        File.AppendAllText(filePath, "\n" + finalLine, Encoding.UTF8);

        Debug.Log("Última entrada agregada al CSV: " + finalLine);
    }
}
