
using UnityEngine;

public class Pinchos : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D colision)
    {
        if (!colision.gameObject.CompareTag("Player")) return;

        if (colision.gameObject.TryGetComponent(out MovimientoJugador jugador))
        {
            jugador.ReaparecerJugador();
            jugador.QuitarVida();
        }
        else
        {
            Debug.LogError("El objeto Player necesita MovimientoJugador en la raíz");
        }
    }
}

