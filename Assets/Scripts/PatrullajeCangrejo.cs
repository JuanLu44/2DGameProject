using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PatrullakeCangrejo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    private Vector2 posicionInicial;
    public Vector2 posicionFinal;

    private bool movimientoHaciaDerecha = true;

    private Rigidbody2D fisica;
    private SpriteRenderer renderizador;

    private void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();
        renderizador = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        posicionInicial = fisica.position;
    }


    private void FixedUpdate()
    {
        Vector2 posicionObjetivo;

        if (movimientoHaciaDerecha)
        {
            posicionObjetivo = posicionFinal;
        }
        else
        {
            posicionObjetivo = posicionInicial;
        }

        Vector2 nuevaPosicion = Vector2.MoveTowards(
            fisica.position,
            posicionObjetivo,
            velocidad * Time.fixedDeltaTime
        );

        fisica.MovePosition(nuevaPosicion);

        if (Vector2.Distance(nuevaPosicion, posicionFinal) < 0.01f)
        {
            movimientoHaciaDerecha = false;
        }
        else if (Vector2.Distance(nuevaPosicion, posicionInicial) < 0.01f)
        {
            movimientoHaciaDerecha = true;
        }

        OrientarSprite();
    }


    private void OrientarSprite()
    {
        if (renderizador == null)
            return;

        if (movimientoHaciaDerecha)
        {
            renderizador.flipX = true;
        }
        else
        {
            renderizador.flipX = false;
        }
    }


    void Update()
    {
        
    }
}
