using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Rigidbody2D))]
public class MovimientoJugador : MonoBehaviour
{

    public float velocity;

    [SerializeField] private InputActionReference accionMover;

    private Rigidbody2D fisica;
    private Vector2 entradaX;

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
        entradaX = accionMover.action.ReadValue<Vector2>();
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
    }

    private void OnDisable()
    {
        if (accionMover != null && accionMover.action != null)
        {
            accionMover.action.Disable();
        }
    }

    private void FixedUpdate()
    {
        fisica.linearVelocity = new Vector2(entradaX.x * velocity, fisica.linearVelocity.y);
    }

}
