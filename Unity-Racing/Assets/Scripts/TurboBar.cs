using UnityEngine;
using UnityEngine.UI;

public class TurboBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float maxValue = 100f;
    public float CurrentValue { get; private set; }

    void Start()
    {
        CurrentValue = maxValue;
        UpdateFill();
    }

    private void UpdateFill()
    {
        if (fillImage != null)
            fillImage.fillAmount = CurrentValue / maxValue;
    }

    public void RemoveValue(float amount)
    {
        CurrentValue = Mathf.Max(CurrentValue - amount, 0f);
        UpdateFill();
    }

    public void AddValue(float amount)
    {
        CurrentValue = Mathf.Min(CurrentValue + amount, maxValue);
        UpdateFill();
    }
}
