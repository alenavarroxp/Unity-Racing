using UnityEngine;

public class TurboController : MonoBehaviour
{
    [Header("Input & UI")]
    public PrometeoTouchInput turboInput;
    public TurboBar turboBar;

    [Header("Rates")]
    [SerializeField] private float consumeRate = 20f;
    [SerializeField] private float rechargeRate = 10f;

    private bool isTurboActive = false;
    private PrometeoCarController carController;

    void Awake()
    {
        if (turboInput == null)
        {
            var go = GameObject.Find("Turbo Button");
            if (go != null)
                turboInput = go.GetComponentInChildren<PrometeoTouchInput>(true);
        }

        if (turboBar == null)
            turboBar = FindObjectOfType<TurboBar>();
    }

    void Update()
    {
        bool pressed = turboInput != null && turboInput.buttonPressed;

        if (pressed && turboBar.CurrentValue > 0f)
        {
            if (!isTurboActive)
            {
                isTurboActive = true;
                ApplyTurboEffect();
            }

            turboBar.RemoveValue(consumeRate * Time.deltaTime);

            if (turboBar.CurrentValue <= 0f)
            {
                EndTurbo();
            }
        }
        else
        {
            if (isTurboActive)
            {
                EndTurbo(); // Turbo fue desactivado manualmente
            }

            turboBar.AddValue(rechargeRate * Time.deltaTime);
        }
    }

    private void ApplyTurboEffect()
    {
        // Aquí tu lógica de turbo (e.g. aumentar velocidad, partículas…)
        GameObject myCar = GameObject.Find("MyCar");
        GameObject secondCar = GameObject.Find("SecondCar");

        bool myCarActive = myCar != null && myCar.activeInHierarchy;
        bool secondCarActive = secondCar != null && secondCar.activeInHierarchy;
    
        if (myCarActive)
        {
            carController = myCar.GetComponent<PrometeoCarController>();
            carController?.ApplyTurboEffect();
        }

        if (secondCarActive)
        {
            carController = secondCar.GetComponent<PrometeoCarController>();
            carController?.ApplyTurboEffect();
        }


    }

    private void EndTurbo()
    {
        isTurboActive = false;
        GameObject myCar = GameObject.Find("MyCar");
        GameObject secondCar = GameObject.Find("SecondCar");

        bool myCarActive = myCar != null && myCar.activeInHierarchy;
        bool secondCarActive = secondCar != null && secondCar.activeInHierarchy;
    
        if (myCarActive)
        {
            carController = myCar.GetComponent<PrometeoCarController>();
            carController?.StopTurboEffect();
        }

        if (secondCarActive)
        {
            carController = secondCar.GetComponent<PrometeoCarController>();
            carController?.StopTurboEffect();
        }
    }
}
