using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class MovimientoJugadorMetodo2 : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocity;

    [SerializeField] private InputActionReference accionMover;


    [Header("Salto")]
    [SerializeField] private InputActionReference accionSaltar;

    public float fuerzaImpulso = 5f;

    [Header("Chequeo de Suelo")]
    [SerializeField] private float groundDistance = 0.2f; //Radio esfera
    [SerializeField] private Transform esferaPies; //Objeto vacío en los pies
    [SerializeField] private LayerMask capaSuelo;

    private Rigidbody2D fisica;
    private Vector2 entradaMovimiento;
    private bool estaTocandoSuelo;
    private bool saltoPendiente;

    private void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        entradaMovimiento = accionMover.action.ReadValue<Vector2>();

        estaTocandoSuelo = Physics2D.OverlapCircle(esferaPies.position, groundDistance, capaSuelo);

        if (accionSaltar.action.WasPressedThisFrame() && estaTocandoSuelo)
        {
            saltoPendiente = true;
        }
    }

    private void FixedUpdate()
    {
        fisica.linearVelocity = new Vector2(entradaMovimiento.x * velocity, fisica.linearVelocity.y);

        if(saltoPendiente)
        {
            fisica.AddForce(Vector2.up * fuerzaImpulso, ForceMode2D.Impulse);
        }
        saltoPendiente = false;
    }

    private void OnEnable()
    {
        if ((accionMover == null) || accionMover.action == null)
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

    private void OnDrawGizmosSelected()
    {
        if(esferaPies!=null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(esferaPies.position, groundDistance);
        }
    }
}
