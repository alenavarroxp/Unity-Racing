using UnityEngine;
using Vuforia;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class GarageManager : MonoBehaviour
{
    public GameObject car1;
    public GameObject car2;
    private GameObject selectedCar;

    private bool isTargetTracked = false;
    private int currentIndex = 0; // 0 = car1, 1 = car2

    public Transform keyObject;
    public Transform statsObject;
    public Vector3 offsetCar1 = new Vector3(-0.45f, 1.484054f, -0.4393685f);
    public Vector3 offsetCar2 = new Vector3(0.45f, 1.484054f, -0.4393685f);
    public Vector3 offsetStats1 = new Vector3(-0.45f, 7.412627f, 0.2363281f);
    public Vector3 offsetStats2 = new Vector3(0.45f, 7.412627f, 0.2363281f);

    public TextMeshPro carNameText;
    public TextMeshPro massText;
    public TextMeshPro maxSpeedText;
    public TextMeshPro reverseSpeedText;
    public TextMeshPro turboText;

    public float moveDuration = 0.5f;
    public Transform carSpawnPoint;
    public Transform carParent;
    public List<GameObject> carVariants;
    public GameObject currentCar;

    private void Start()
    {
        var observer = GetComponent<ObserverBehaviour>();
        if (observer != null)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        // Ambos activos siempre
        car1.SetActive(true);
        car2.SetActive(true);

        if (currentCar != null)
        {
            if (currentCar.name.ToLower().Contains("prometheus"))
                SelectCar(1); // PROMETEO
            else
                SelectCar(0); // FREE RACING CAR
        }
        else
        {
            SelectCar(0);
        }
    }

    private void Update()
    {
        if (isTargetTracked && selectedCar != null)
        {
            Vector3 currentRotation = selectedCar.transform.localRotation.eulerAngles;
            selectedCar.transform.localRotation = Quaternion.Euler(0f, currentRotation.y + 40 * Time.deltaTime, 0f);
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        isTargetTracked = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;
    }

    public void SelectCar(int index)
    {
        selectedCar = index == 0 ? car1 : car2;
        currentIndex = index;

        // Reset rotations
        car1.transform.localRotation = Quaternion.Euler(0, 0, 0);
        car2.transform.localRotation = Quaternion.Euler(0, 0, 0);

        // Mover key y stats
        Vector3 targetKeyLocalPos = index == 0 ? offsetCar1 : offsetCar2;
        Vector3 targetStatsLocalPos = index == 0 ? offsetStats1 : offsetStats2;
        StopAllCoroutines();
        StartCoroutine(MoveKeySmoothly(targetKeyLocalPos));
        StartCoroutine(MoveStatsSmoothly(targetStatsLocalPos));

        UpdateCarStatsUI();
    }

    public void NextCar()
    {
        int newIndex = (currentIndex + 1) % 2;
        SelectCar(newIndex);
    }

    public void PrevCar()
    {
        int newIndex = (currentIndex - 1 + 2) % 2; // Maneja el -1
        SelectCar(newIndex);
    }

    private IEnumerator MoveKeySmoothly(Vector3 targetLocalPos)
    {
        Vector3 startKeyLocalPos = keyObject.localPosition;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            keyObject.localPosition = Vector3.Lerp(startKeyLocalPos, targetLocalPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        keyObject.localPosition = targetLocalPos;
    }

    private IEnumerator MoveStatsSmoothly(Vector3 targetLocalPos)
    {
        Vector3 startStatsLocalPos = statsObject.localPosition;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            statsObject.localPosition = Vector3.Lerp(startStatsLocalPos, targetLocalPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        statsObject.localPosition = targetLocalPos;
    }

    private void UpdateCarStatsUI()
    {
        if (currentIndex == 0)
        {
            carNameText.text = "ARCADE - Free Racing Car";
            massText.text = "1000kg";
            maxSpeedText.text = "100kmh";
            reverseSpeedText.text = "35kmh";
            turboText.text = "Yes";
        }
        else
        {
            carNameText.text = "PROMETEO";
            massText.text = "900kg";
            maxSpeedText.text = "120kmh";
            reverseSpeedText.text = "50kmh";
            turboText.text = "No";
        }
    }

    public void ConfirmCar()
    {
        // Guarda la posición y rotación global del coche actual antes de cambiarlo
        Vector3 currentCarWorldPosition = currentCar.transform.position;
        Quaternion currentCarWorldRotation = currentCar.transform.rotation;

        // Activa el coche correspondiente al índice actual
        for (int i = 0; i < carVariants.Count; i++)
        {
            carVariants[i].SetActive(i == currentIndex);
        }

        // Actualiza currentCar
        currentCar = carVariants[currentIndex];

        // Restaura la posición y rotación global al coche seleccionado
        currentCar.transform.position = currentCarWorldPosition;
        currentCar.transform.rotation = currentCarWorldRotation;
    }

}
