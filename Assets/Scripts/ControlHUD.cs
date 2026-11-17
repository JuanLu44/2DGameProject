using UnityEngine;
using TMPro;

public class ControlHUD : MonoBehaviour
{
    public TMP_Text vidasTxt;
    public TMP_Text coleccionablesTxt;
    public TMP_Text tiempoTxt;

    private void Awake()
    {
        if (vidasTxt == null || coleccionablesTxt == null || tiempoTxt == null)
            Debug.LogError("Asigna los textos TMP del HUD al componente ControlHUD");
    }

    public void SetColeccionables(int cantidad)
    {
        if (coleccionablesTxt != null)
        {
            coleccionablesTxt.text = "Coleccionables: " + cantidad;
        }
    }

    public void SetVidas(int cantidad)
    {
        if (vidasTxt != null)
        {
            vidasTxt.text = "Vidas: " + cantidad;
        }
    }

    public void SetTiempo(int segundosRestantes)
    {
        if (tiempoTxt == null) return;

        int minutos = segundosRestantes / 60;
        int segundos = segundosRestantes % 60;

        tiempoTxt.text = minutos.ToString("00") + ":" + segundos.ToString("00");
        
    }
}
