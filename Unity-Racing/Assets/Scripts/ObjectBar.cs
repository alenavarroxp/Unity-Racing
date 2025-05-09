using UnityEngine;
using UnityEngine.UI;

public class ObjectBar : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField, Min(0.1f)] float changeSpeed = 100;
    [SerializeField] float maxValue = 100;
    [SerializeField] float objectValue = 10;

    [SerializeField] Canvas canvas;
    [SerializeField] Canvas EndCanvas;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource finishSound;

    [SerializeField] ObjectManager objectManager;
    [SerializeField] Text endObjectsText;
    [SerializeField] TimerController timer;
    [SerializeField] Text endTimeText;


    public Text quantityText;
    float currentValue;
    float displayValue;

    void Start()
    {
        currentValue = 0;
        displayValue = 0;
    }

    void Update()
    {
        displayValue = Mathf.MoveTowards(displayValue, currentValue, Time.deltaTime * changeSpeed);
        UpdateFill();
    }

    void UpdateFill()
    {
        float value = Mathf.InverseLerp(0, maxValue, displayValue);
        fillImage.fillAmount = value;
    }

    public void AddObject()
    {
        currentValue = Mathf.Min(currentValue + objectValue, maxValue);
        quantityText.text = (currentValue / objectValue).ToString("0");

        if(currentValue == maxValue)
        {
            canvas.gameObject.SetActive(false);
            EndCanvas.gameObject.SetActive(true);

            int count = objectManager.GetCountObjects();
            string label = count == 1 ? " objeto" : " objetos";
            endObjectsText.text = count.ToString() + label;

            audioSource.Stop();
            finishSound.Play();
            objectManager.GetCountObjects();

            timer.Pausar();
            endTimeText.text = timer.GetTiempoFormateado();
        }
    }

    public void ResetBar()
    {
        currentValue = 0;
        quantityText.text = "0";
    }
}
