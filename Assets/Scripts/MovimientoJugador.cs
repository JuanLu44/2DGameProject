using UnityEngine;
using UnityEngine.InputSystem;
using System;

[RequireComponent (typeof(Rigidbody2D))]
public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocity;

    [SerializeField] private InputActionReference accionMover;


    [Header("Salto")]
    [SerializeField] private InputActionReference accionSaltar;

    public float fuerzaImpulso = 5f;
    [SerializeField] private LayerMask capaSuelo;

    [Header("Spawn-Point")]
    [SerializeField] private Transform spawnPoint;

    [Header("Puntuación")]
    [SerializeField] private int puntuacion;
    [SerializeField] private int coleccionablesRestantes;

    [Header("Salud")]
    [SerializeField] private int vidas = 3;
    [SerializeField, Min(0.01f)] private float segundosInvulnerable = 1f;
    public int Vidas => vidas;
    private Color colorOriginal;
    
    [Header("Tiempo")]
    [SerializeField] private int tiempoEmpleado;
    [SerializeField, Min(1)] private int tiempoLimiteNivel = 100;
    private float tiempoInicio;
    private float tiempoReal;

    [Header("Canvas")]
    [SerializeField] private Canvas canvasInterfaz;
    private ControlHUD hud;
    

    private Rigidbody2D fisica;
    private SpriteRenderer renderizador;
    private Animator animador;
    private Vector2 entradaX;
    private bool saltoPendiente;
    private bool estaTocandoSuelo;
    private float longitudRayCast = 1.6f;
    private float umbralCarrera = 0.1f;

    private void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();

        renderizador = GetComponent<SpriteRenderer>();

        animador = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (renderizador != null)
            colorOriginal = renderizador.color;
        else
            Debug.LogError("El jugador no tiene asignado el objeto Sprite Renderer.");

        tiempoInicio = Time.time;

        InicializaHUD();
    }

    // Update is called once per frame
    void Update()
    {
        entradaX = accionMover.action.ReadValue<Vector2>();

        ComprobarFinJuegoTiempo();

        SaltoJugador();

        AnimarJugador();
    }

    private void InicializaHUD()
    {
        if (canvasInterfaz != null)
        {
            hud = canvasInterfaz.GetComponent<ControlHUD>();
        }
        else
            Debug.LogError("Asigna el objeto Canvas con el componente ControlHUD al atributo Canvas del jugador.");

        if (hud != null)
        {
            hud.SetVidas(vidas);
            hud.SetTiempo(tiempoLimiteNivel);
            coleccionablesRestantes = GameObject.FindGameObjectsWithTag("Coleccionable").Length;
            hud.SetColeccionables(coleccionablesRestantes);
        }
        else
            Debug.LogError("Asigna el objeto Canvas con el componente ControlHUD al atributo Canvas del jugador.");


    }   


    private void ComprobarFinJuegoTiempo()
    {
        tiempoReal = Time.time - tiempoInicio;
        tiempoEmpleado = Mathf.FloorToInt(tiempoReal);
        int segundosRestantes = Mathf.CeilToInt(tiempoLimiteNivel - tiempoReal);
        hud.SetTiempo(segundosRestantes);

        if (tiempoEmpleado >= tiempoLimiteNivel)
        {
            FinJuego();
            return;
        }
    }

    private void SaltoJugador()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRayCast, capaSuelo);

        estaTocandoSuelo = hit.collider != null;

        Color colorRayo = estaTocandoSuelo ? Color.green : Color.red;

        Debug.DrawRay(transform.position, Vector2.down * longitudRayCast, colorRayo);

        if (accionSaltar.action.WasPressedThisFrame() && estaTocandoSuelo)
        {
            saltoPendiente = true;
        }
    }

    private void AnimarJugador()
    {
        if (animador == null)
            return;

        if (!estaTocandoSuelo)
        {
            animador.Play("JugadorSaltando");
        }
        else if (Math.Abs(fisica.linearVelocity.x) > umbralCarrera)
        {
            animador.Play("JugadorCorriendo");
        }
        else
            animador.Play("JugadorParado");
    }

    private void OnEnable()
    {
        if (accionMover == null || accionMover.action == null)
        {
            Debug.LogError("Asigna Mapa de acción Jugador/Mover en Acción Mover");
            enabled = false;
            return;
        }

        accionMover.action.Enable();

        if (accionSaltar == null || accionSaltar.action == null)
        {
            Debug.LogError("Asigna Mapa de acción Jugador/Saltar en Acción Saltar");
            enabled = false;
            return;
        }

        accionSaltar.action.Enable();
    }

    private void OnDisable()
    {
        if (accionMover != null && accionMover.action != null)
        {
            accionMover.action.Disable();
        }

        if (accionSaltar != null && accionSaltar.action != null)
        {
            accionSaltar.action.Disable();
        }
    }

    private void FixedUpdate()
    {
        MoverJugador();

        OrientarSprite();
    }

    private void MoverJugador()
    {
        fisica.linearVelocity = new Vector2(entradaX.x * velocity, fisica.linearVelocity.y);

        if (saltoPendiente)
        {
            fisica.AddForce(Vector2.up * fuerzaImpulso, ForceMode2D.Impulse);
        }

        saltoPendiente = false;
    }

    private void OrientarSprite()
    {
        if (renderizador == null)
            return;

        if (fisica.linearVelocity.x > 0)
        {
            renderizador.flipX = false;
        } 
        else if (fisica.linearVelocity.x < 0)
        {
            renderizador.flipX = true;
        }
    }

    public void ReaparecerJugador()
    {
        if (spawnPoint == null)
        {
            Debug.LogError(
                "Asigna un Spawn Point al objeto jugador.", this);
            enabled = false;
            return;
        }


        fisica.position = (Vector2)spawnPoint.position;
        fisica.linearVelocity = Vector2.zero;
    }

    public void IncrementarPuntos(int cantidad)
    {
        if (cantidad <= 0) return;

        puntuacion += cantidad;
    }

    public void QuitarVida()
    {

        vidas--;
        hud.SetVidas(vidas);

        if (renderizador != null)
        {
            renderizador.color = Color.blue;
        }

        if (vidas <= 0)
        {
            FinJuego();
            return;
        }

        Invoke(nameof(HacerVulnerable), segundosInvulnerable);
    }

    public void DecrementarColeccionables()
    {
        if (coleccionablesRestantes <= 0) return;

        coleccionablesRestantes--;

        hud.SetColeccionables(coleccionablesRestantes);
    }

    private void HacerVulnerable()
    {
        if (renderizador != null)
            renderizador.color = colorOriginal;
    }


    private void FinJuego()
    {
        Debug.Log("Juego finalizado.");
        gameObject.SetActive(false);
    }

}
