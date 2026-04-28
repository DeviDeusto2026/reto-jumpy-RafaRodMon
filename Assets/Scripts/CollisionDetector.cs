using UnityEngine;

/// CollisionDetector.cs
/// Adjuntado al ZORRO. Gestiona la interacción con las GhostingPlatform:
/// - Mientras el zorro sube o está por debajo de la plataforma, es atravesable.
/// - Cuando el zorro cae sobre ella, se vuelve sólida.
/// NUNCA afecta al suelo principal (este no debe tener GhostingPlatform).

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        propioCollider = GetComponent<Collider>();
    }

    /// Calcula la mitad de la altura del collider en tiempo real
    private float MitadAltura => propioCollider.bounds.extents.y;

    private void Update()
    {
        GhostingPlatform[] plataformas = FindObjectsByType<GhostingPlatform>(FindObjectsSortMode.None);

        foreach (GhostingPlatform plataforma in plataformas)
        {
            // SEGURIDAD: solo procesa objetos con el tag correcto.
            // Evita que el suelo u otros objetos sean afectados
            // aunque accidentalmente tengan GhostingPlatform.
            if (!plataforma.CompareTag(tagPlataforma))
            {
                plataforma.DesactivarModoFantasma(propioCollider);
                continue;
            }

            ActualizarEstadoPlataforma(plataforma);
        }
    }

    /// Decide si la plataforma debe ser sólida o atravesable para el zorro.
    private void ActualizarEstadoPlataforma(GhostingPlatform plataforma)
    {
        float mitad = MitadAltura;
        float pieZorro = transform.position.y - mitad;
        float techoPlatforma = plataforma.ColisionadorPlataforma.bounds.max.y;

        bool zorroEstaArriba = pieZorro >= techoPlatforma - margenSolidez;
        bool zorroEstaEnCaida = rb.linearVelocity.y <= umbralVelocidadCaida;

        if (zorroEstaArriba || (zorroEstaEnCaida && !plataforma.ZorroVieneDesdeAbajo(transform, mitad)))
        {
            plataforma.DesactivarModoFantasma(propioCollider);
        }
       
    }
}