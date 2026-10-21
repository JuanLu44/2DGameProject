using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ControlEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;

    private Vector2 posicionInicial;
    public Vector2 posicionFinal;

    private bool movimientoHaciaArriba = true;

    [SerializeField] private float tolerancia = 0.2f;

    private Rigidbody2D fisica;

    private void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        posicionInicial = fisica.position;
    }

    private void FixedUpdate()
    {
        if (velocidad <= 0)
            return;

        Vector2 posicionObjetivo = movimientoHaciaArriba
            ? posicionFinal
            : posicionInicial;

        // Comprobamos si ha llegado al extremo actual.
        if (Mathf.Abs(fisica.position.y - posicionObjetivo.y) <= tolerancia)
        {
            movimientoHaciaArriba = !movimientoHaciaArriba;

            posicionObjetivo = movimientoHaciaArriba
                ? posicionFinal
                : posicionInicial;
        }

        Vector2 nuevaPosicion = Vector2.MoveTowards(
            fisica.position,
            posicionObjetivo,
            velocidad * Time.fixedDeltaTime
        );

        fisica.MovePosition(nuevaPosicion);
    }
}