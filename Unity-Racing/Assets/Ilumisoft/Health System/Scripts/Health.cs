using UnityEngine;
using UnityEngine.UI; // Asegúrate de agregar esta línea

namespace Ilumisoft.HealthSystem
{
    [AddComponentMenu("Health System/Health")]
    public class Health : HealthComponent
    {
        [Tooltip("The max amount of health that can be assigned")]
        [SerializeField]
        private float maxHealth = 100.0f;

        [Tooltip("The initial amount of health assigned")]
        [SerializeField, Range(0, 1)]
        private float initialRatio = 1.0f;

        /// <summary>
        /// Is health currently being consumed (e.g. fuel being used)?
        /// </summary>
        public bool isConsuming = false;

        [SerializeField] private Image healthImage;

        public override float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public override float CurrentHealth { get; set; } = 0.0f;
        public override bool IsAlive => CurrentHealth > 0.0f;

        public PrometeoTouchInput throttleInput;
        public PrometeoTouchInput backInput;

        private void Awake()
        {
            SetHealth(maxHealth * initialRatio);

            GameObject touchInputObject = GameObject.Find("Throttle Button");
            GameObject backInputObject = GameObject.Find("Brakes/Reverse Button");
            if (touchInputObject != null && backInputObject != null)
            {
                throttleInput = touchInputObject.GetComponent<PrometeoTouchInput>();
                backInput = backInputObject.GetComponent<PrometeoTouchInput>();
                if (throttleInput == null || backInput == null)
                {
                    Debug.LogError("No se encontró el componente PrometeoTouchInput en el objeto Throttle Button o Brakes/Reverse Button.");
                }
            }
            else
            {
                Debug.LogError("No se encontró el objeto TouchInput en la escena.");
            }
        }

        private float consumeTimer = 0f;

        private void Update()
        {
            if (isConsuming && IsAlive)
            {
                consumeTimer += Time.deltaTime;

                if (consumeTimer >= 0.01f)
                {
                    ApplyDamage(0.2f);
                    consumeTimer = 0f;
                }
            }
            if(!IsAlive){
                Debug.Log("NOT FUEL is empty, stopping consumption.");
                isConsuming = false;
                throttleInput.SetIsOutOfFuel(true);
                backInput.SetIsOutOfFuel(true); 

                throttleInput.ForceButtonUp();
                backInput.ForceButtonUp();
            }
        }

        public override void SetHealth(float health)
        {
            float previousHealth = CurrentHealth;

            CurrentHealth = Mathf.Clamp(health, 0, MaxHealth);

            float difference = health - previousHealth;

            if (difference > 0.0f)
            {
                OnHealthChanged?.Invoke(difference);
            }

            UpdateHealthColor(); // Actualiza el color al cambiar la salud
        }

        public override void AddHealth(float amount)
        {
            float previousHealth = CurrentHealth;

            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

            float changeAmount = CurrentHealth - previousHealth;

            if (changeAmount > 0.0f)
            {
                OnHealthChanged?.Invoke(changeAmount);
            }

            UpdateHealthColor(); // Actualiza el color al añadir salud
        }

        public override void ApplyDamage(float damage)
        {
            if (!IsAlive) return;

            float previousHealth = CurrentHealth;

            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);

            float changeAmount = CurrentHealth - previousHealth;

            if (Mathf.Abs(changeAmount) > 0.0f)
            {
                OnHealthChanged?.Invoke(changeAmount);

                if (CurrentHealth <= 0.0f)
                {
                    OnHealthEmpty?.Invoke();
                }
            }

            UpdateHealthColor(); // Actualiza el color al recibir daño
        }

        private void UpdateHealthColor()
        {
            if (healthImage == null) return;

            Color healthColor;

            if (CurrentHealth > 70)
            {
                healthColor = Color.green;
            }
            else if (CurrentHealth > 40)
            {
                healthColor = new Color(1.0f, 0.6f, 0.0f);
            }
            else
            {
                healthColor = Color.red;
            }

            healthImage.color = healthColor;
        }

        public void ToggleConsuming()
        {
            isConsuming = !isConsuming;
        }

        public void RefillHealth()
        {
            float missingHealth = MaxHealth - CurrentHealth;
            Debug.Log("RefillHealth called: " + missingHealth);
            if (missingHealth > 0)
            {
                AddHealth(missingHealth);
                throttleInput.SetIsOutOfFuel(false);
                backInput.SetIsOutOfFuel(false);
            }
        }

    }
}
