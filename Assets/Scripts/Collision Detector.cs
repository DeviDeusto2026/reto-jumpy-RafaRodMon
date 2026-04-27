using UnityEngine;

/// Adjuntado al ZORRO. Gestiona la interacción con las GhostingPlatform:
/// - Mientras el zorro sube (velocidad Y positiva) o está por debajo de la plataforma,
///   la plataforma es atravesable.
/// - Cuando el zorro cae sobre ella, se vuelve sólida.
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class CollisionDetector : MonoBehaviour
{
    public string tagPlataforma = "Platform";
    public float umbralVelocidadCaida = -0.1f;
    public float margenSolidez = 0.05f;

    // Componentes propios
    private Rigidbody rb;
    private Collider propioCollider;

    // Altura media del zorro (para detectar si el pie está sobre la plataforma)
    private float mitadAltura;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        propioCollider = GetComponent<Collider>();
        mitadAltura = propioCollider.bounds.extents.y;
    }

    private void Update()
    {
        // Busca todas las plataformas activas y actualiza su estado
        // (Más eficiente sería usar triggers o eventos, pero esto es claro y mantenible)
        GhostingPlatform[] plataformas = FindObjectsByType<GhostingPlatform>(FindObjectsSortMode.None);

        foreach (GhostingPlatform plataforma in plataformas)
        {
            ActualizarEstadoPlataforma(plataforma);
        }
    }
    /// Decide si la plataforma debe ser sólida o atravesable para el zorro.
    private void ActualizarEstadoPlataforma(GhostingPlatform plataforma)
    {
        float pieZorro = transform.position.y - mitadAltura;
        float techoPlatforma = plataforma.ColisionadorPlataforma.bounds.max.y;

        bool zorroEstaArriba = pieZorro >= techoPlatforma - margenSolidez;
        bool zorroEstaEnCaida = rb.linearVelocity.y <= umbralVelocidadCaida;

        if (zorroEstaArriba || (zorroEstaEnCaida && !plataforma.ZorroVieneDesdeAbajo(transform, mitadAltura)))
        {
            // Zorro encima o cayendo sobre ella: sólida
            plataforma.DesactivarModoFantasma(propioCollider);
        }
        else
        {
            // Zorro por debajo o subiendo: atravesable
            plataforma.ActivarModoFantasma(propioCollider);
        }
    }
    /// Devuelve la mitad de la altura del zorro (para cálculos externos).
    public float MitadAltura => mitadAltura;
}