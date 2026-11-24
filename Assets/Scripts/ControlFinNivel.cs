using UnityEngine;
using TMPro;

public class ControlFinNivel : MonoBehaviour
{
    [SerializeField] private TMP_Text mensajeFinal;

    // Start is called once before the first execution of Update
    void Start()
    {
        if (mensajeFinal == null)
        {
            Debug.LogError("Asigna el TextMeshPro a Mensaje Final");
            return;
        }

        ControlDatosJuego datosJuego = ControlDatosJuego.Instancia;

        mensajeFinal.text = datosJuego.HaGanado
            ? "¡Has Ganado!\nPuntuación obtenida: " + datosJuego.Puntuacion
            : "Lo siento. Has perdido.";
    }
}
