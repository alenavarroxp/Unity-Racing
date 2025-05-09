using UnityEngine;

public class TimerController : MonoBehaviour
{
    private float tiempo;
    private bool enMarcha = false;

    public void Iniciar()
    {
        tiempo = 0f;
        enMarcha = true;
    }

    public void Pausar()
    {
        enMarcha = false;
    }

    public void Reanudar()
    {
        enMarcha = true;
    }

    public void Reiniciar()
    {
        tiempo = 0f;
        enMarcha = false;
    }

    public float GetTiempo()
    {
        return tiempo;
    }

    public string GetTiempoFormateado()
    {
        int minutos = Mathf.FloorToInt(tiempo / 60f);
        int segundos = Mathf.FloorToInt(tiempo % 60f);
        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    void Update()
    {
        if (enMarcha)
        {
            tiempo += Time.deltaTime;
        }
    }
}
