using UnityEngine;

public class ControlColeccionable : MonoBehaviour
{
    [SerializeField, Range(1, 10)] private int cantidad = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (collision.gameObject.TryGetComponent(out MovimientoJugador jugador)) 
        {
            //Llamar al método público que incrementa la puntuación.
            jugador.IncrementarPuntos(cantidad);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("El objeto Player necesita un componente MovimientoJugador.");
        }
    } 
}
