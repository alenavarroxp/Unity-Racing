using UnityEngine;
using UnityEngine.UI;

public class ObjectBar : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField, Min(0.1f)] float changeSpeed = 100;
    [SerializeField] float maxValue = 100;
    [SerializeField] float objectValue = 10;

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
            Debug.Log("Bar is full!");
        }
    }

    public void ResetBar()
    {
        currentValue = 0;
        quantityText.text = "0";
    }
}
