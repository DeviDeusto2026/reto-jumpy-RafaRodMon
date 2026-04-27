using UnityEngine;

/// Controla el movimiento horizontal del zorro y su salto.
/// El zorro solo puede saltar si est� en el suelo (o sobre una plataforma).
/// No puede volver a saltar hasta que toque el suelo.
[RequireComponent(typeof(Rigidbody))]
public class FoxMovement : MonoBehaviour
{
    public float velocidadMovimiento = 6f;
    public float fuerzaSalto = 12f;
    public float multiplicadorCaida = 2.5f;
    public float multiplicadorSaltoCorto = 2f;
    public Transform puntoDeteccionSuelo;
    public float distanciaDeteccionSuelo = 0.15f;
    public LayerMask capaSuelo;

    // Componentes
    private Rigidbody rb;
    private bool estaEnSuelo;
    private bool saltoPresionado;

    // Direcci�n horizontal de entrada
    private float entradaHorizontal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Configuración del Rigidbody para comportamiento 2D en 3D
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    private void Update()
    {
        // Leer entradas
        entradaHorizontal = Input.GetAxisRaw("Horizontal");
        saltoPresionado = Input.GetButtonDown("Jump");

        // Detección de suelo
        estaEnSuelo = DetectarSuelo();

    }

    private void FixedUpdate()
    {
        AplicarMovimientoHorizontal();
        AplicarGravedadMejorada();

        if (saltoPresionado && estaEnSuelo)
        {
            Saltar();
            saltoPresionado = false;
        }
    }

    /// Mueve el zorro lateralmente según la entrada del jugador.
    private void AplicarMovimientoHorizontal()
    {
        Vector3 velocidad = rb.linearVelocity;
        velocidad.x = entradaHorizontal * velocidadMovimiento;
        rb.linearVelocity = velocidad;

        // Voltear el sprite/modelo según dirección
        if (entradaHorizontal != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(entradaHorizontal) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    /// Gravedad mejorada: caida mas pesada, salto corto al soltar el boton.
    private void AplicarGravedadMejorada()
    {
        if (rb.linearVelocity.y < 0)
        {
            // Caida: gravedad extra para sentirse mas natural
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (multiplicadorCaida - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Salto corto: si se suelta el boton, el salto es mas bajo
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (multiplicadorSaltoCorto - 1) * Time.fixedDeltaTime;
        }
    }

    /// Aplica la fuerza de salto.

    private void Saltar()
    {
        // Resetea la velocidad vertical antes de saltar para saltos consistentes
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
    }

    /// Detecta si el zorro esta sobre el suelo o una plataforma solida.
    private bool DetectarSuelo()
    {
        Vector3 origen = puntoDeteccionSuelo != null
            ? puntoDeteccionSuelo.position
            : transform.position;

        return Physics.Raycast(origen, Vector3.down, distanciaDeteccionSuelo, capaSuelo);
    }

    /// Devuelve si el zorro esta actualmente en el suelo (para uso externo).
    public bool EstaEnSuelo => estaEnSuelo;

    /// Resetea el zorro a una posicion dada (usado al reiniciar).
    public void Resetear(Vector3 posicion)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = posicion;
        estaEnSuelo = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualiza el raycast de detecci�n de suelo en el editor
        Vector3 origen = puntoDeteccionSuelo != null
            ? puntoDeteccionSuelo.position
            : transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origen, origen + Vector3.down * distanciaDeteccionSuelo);
    }
}