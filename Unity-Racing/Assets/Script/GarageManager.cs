using UnityEngine;
using Vuforia;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GarageManager : MonoBehaviour
{
    public GameObject car1;
    public GameObject car2;
    private GameObject selectedCar;

    private bool isTargetTracked = false;
    private int currentIndex = 0; // 0 = car1, 1 = car2

    [Header("UI Stats")]
    public TextMeshPro carNameText;
    public TextMeshPro massText;
    public TextMeshPro maxSpeedText;
    public TextMeshPro reverseSpeedText;
    public TextMeshPro turboText;

    [Header("Speed UI")]
    public GameObject speedTextCar1; // GameObject que contiene el TextMeshProUGUI del coche 1
    public GameObject speedTextCar2; // GameObject que contiene el TextMeshProUGUI del coche 2

    [Header("Movement")]
    public Transform keyObject;
    public Transform statsObject;
    public Vector3 offsetCar1 = new Vector3(-0.45f, 1.484054f, -0.4393685f);
    public Vector3 offsetCar2 = new Vector3(0.45f, 1.484054f, -0.4393685f);
    public Vector3 offsetStats1 = new Vector3(-0.45f, 7.412627f, 0.2363281f);
    public Vector3 offsetStats2 = new Vector3(0.45f, 7.412627f, 0.2363281f);
    public float moveDuration = 0.5f;

    [Header("Cars")]
    public List<GameObject> carVariants;
    public GameObject currentCar;

    [Header("Effects & Audio")]
    public GameObject carRepairParticlesPrefab;
    private AudioSource audioSource;
    public GameObject selectButton;

    private void Start()
    {
        // Vuforia setup
        var observer = GetComponent<ObserverBehaviour>();
        if (observer != null) observer.OnTargetStatusChanged += OnTargetStatusChanged;

        // Ambos activos para tracking
        car1.SetActive(true);
        car2.SetActive(true);

        // Selección inicial
        currentIndex = (currentCar != null && currentCar.name.ToLower().Contains("prometheus")) ? 1 : 0;
        SelectCar(currentIndex);
        currentCar = carVariants[currentIndex];

        // AudioSource
        var soundObj = GameObject.Find("Drill Sound");
        if (soundObj != null) audioSource = soundObj.GetComponent<AudioSource>();
        else Debug.LogWarning("AudioSource 'Drill Sound' no encontrado.");
    }

    private void Update()
    {
        if (isTargetTracked && selectedCar != null)
        {
            Vector3 rot = selectedCar.transform.localRotation.eulerAngles;
            selectedCar.transform.localRotation = Quaternion.Euler(0, rot.y + 40f * Time.deltaTime, 0);
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour b, TargetStatus status)
    {
        isTargetTracked = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;
    }

    public void SelectCar(int index)
    {
        currentIndex = index;
        selectedCar = (index == 0) ? car1 : car2;

        // Resetea rotaciones
        car1.transform.localRotation = Quaternion.identity;
        car2.transform.localRotation = Quaternion.identity;

        // Mueve key y stats
        Vector3 keyPos = (index == 0) ? offsetCar1 : offsetCar2;
        Vector3 statsPos = (index == 0) ? offsetStats1 : offsetStats2;
        StopAllCoroutines();
        StartCoroutine(MoveKeySmoothly(keyPos));
        StartCoroutine(MoveStatsSmoothly(statsPos));

        // Actualiza panel de datos
        UpdateCarStatsUI();

        // Controla los dos Speed Texts
        speedTextCar1.SetActive(index == 0);
        speedTextCar2.SetActive(index == 1);
    }

    public void NextCar() => SelectCar((currentIndex + 1) % carVariants.Count);
    public void PrevCar() => SelectCar((currentIndex - 1 + carVariants.Count) % carVariants.Count);

    private IEnumerator MoveKeySmoothly(Vector3 target)
    {
        Vector3 start = keyObject.localPosition;
        float t = 0f;
        while (t < moveDuration)
        {
            keyObject.localPosition = Vector3.Lerp(start, target, t / moveDuration);
            t += Time.deltaTime;
            yield return null;
        }
        keyObject.localPosition = target;
    }

    private IEnumerator MoveStatsSmoothly(Vector3 target)
    {
        Vector3 start = statsObject.localPosition;
        float t = 0f;
        while (t < moveDuration)
        {
            statsObject.localPosition = Vector3.Lerp(start, target, t / moveDuration);
            t += Time.deltaTime;
            yield return null;
        }
        statsObject.localPosition = target;
    }

    private void UpdateCarStatsUI()
    {
        if (currentIndex == 0)
        {
            carNameText.text      = "ARCADE - Free Racing Car";
            massText.text         = "1000kg";
            maxSpeedText.text     = "100kmh";
            reverseSpeedText.text = "35kmh";
            turboText.text        = "Yes";
        }
        else
        {
            carNameText.text      = "PROMETEO";
            massText.text         = "900kg";
            maxSpeedText.text     = "120kmh";
            reverseSpeedText.text = "50kmh";
            turboText.text        = "No";
        }
    }

    public void ConfirmCar()
    {
        if (audioSource != null && audioSource.isPlaying)
            return;

        CancelInvoke();
        if (selectButton != null)
            StartCoroutine(DisableButtonTemporarily(1.5f));

        if (currentCar != null)
        {
            Vector3 wp = currentCar.transform.position;
            Quaternion wr = currentCar.transform.rotation;

            // Activa solo el coche actual
            for (int i = 0; i < carVariants.Count; i++)
                carVariants[i].SetActive(i == currentIndex);

            // Actualiza currentCar y restaura transform
            currentCar = carVariants[currentIndex];
            currentCar.transform.position = wp;
            currentCar.transform.rotation = wr;
        }

        audioSource?.Play();
        if (carRepairParticlesPrefab != null && currentCar != null)
        {
            var ps = Instantiate(carRepairParticlesPrefab, currentCar.transform.position, Quaternion.identity);
            ps.transform.SetParent(currentCar.transform);
            Destroy(ps, 3f);
        }
    }

    private IEnumerator DisableButtonTemporarily(float s)
    {
        if (selectButton == null) yield break;
        selectButton.SetActive(false);
        yield return new WaitForSeconds(s);
        selectButton.SetActive(true);
    }
}
