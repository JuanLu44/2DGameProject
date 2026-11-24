using UnityEngine;

public class ControlColeccionable : MonoBehaviour
{
    [SerializeField, Range(1, 10)] private int cantidad = 2;

    [SerializeField] private AudioClip recogerSonido;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (collision.gameObject.TryGetComponent(out MovimientoJugador jugador)) 
        {
            //Llamar al método público que incrementa la puntuación.
            jugador.IncrementarPuntos(cantidad);
            jugador.DecrementarColeccionables();

            bool esUltimo = GameObject.FindGameObjectsWithTag("Coleccionable").Length == 1;

            AudioSource audioJugador = jugador.GetComponent<AudioSource>();
            if(recogerSonido != null && audioJugador != null)
                audioJugador.PlayOneShot(recogerSonido);

            Destroy(gameObject);

            if (esUltimo)
                jugador.Victoria();
        }
        else
        {
            Debug.LogError("El objeto Player necesita un componente MovimientoJugador.");
        }
    } 
}
