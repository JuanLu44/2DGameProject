using UnityEngine;

public class ControlDatosJuego : MonoBehaviour
{
    public static ControlDatosJuego Instancia {  get; private set; }

    private int puntuacion;

    private bool haGanado;

    public int Puntuacion => puntuacion;
    public bool HaGanado => haGanado;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ReiniciarPartida()
    {
        puntuacion = 0;
        haGanado = false;
    }

    public void GuardarResultado(bool gano, int puntos)
    {
        haGanado = gano;
        puntuacion = gano ? puntos : 0;
    }
}
