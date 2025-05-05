using UnityEngine;
using Vuforia;
using System.Collections;


public class GarageManager : MonoBehaviour
{
    public GameObject car1;
    public GameObject car2;
    public GameObject selectedCar;

    private bool isTargetTracked = false;
    private int currentIndex = 0; // 0 = car1, 1 = car2

    public Transform keyObject;
    public Vector3 offsetCar1 = new Vector3(-0.45f, 0f, 0f);
    public Vector3 offsetCar2 = new Vector3(0.45f, 0f, 0f);
    public float moveDuration = 0.5f;

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

        SelectCar(0);
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

        car1.transform.localRotation = Quaternion.Euler(0, 0, 0);
        car2.transform.localRotation = Quaternion.Euler(0, 0, 0);

        Vector3 targetLocalPos = index == 0 ? offsetCar1 : offsetCar2;
        StopAllCoroutines();
        StartCoroutine(MoveKeySmoothly(targetLocalPos));
    }

    public void NextCar()
    {
        int newIndex = (currentIndex + 1) % 2;
        SelectCar(newIndex);
    }

    public void PrevCar()
    {
        int newIndex = (currentIndex - 1) % 2;
        SelectCar(newIndex);
    }

    private IEnumerator MoveKeySmoothly(Vector3 targetLocalPos)
    {
        Vector3 startLocalPos = keyObject.localPosition;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            keyObject.localPosition = Vector3.Lerp(startLocalPos, targetLocalPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        keyObject.localPosition = targetLocalPos;
    }

}
