using UnityEngine;

/// GhostingPlatform.cs
/// Hace que la plataforma sea atravesable cuando el zorro viene desde abajo,
/// pero sólida cuando el zorro cae encima de ella.
/// Requiere un Collider en la plataforma y que el zorro tenga un Collider.
/// Trabaja junto a CollisionDetector.cs.
[RequireComponent(typeof(Collider))]
public class GhostingPlatform : MonoBehaviour
{
    public string tagJugador = "Player";
    public float margenDeteccion = 0.05f;

    // Referencia al collider de esta plataforma
    private Collider colisionadorPlataforma;

    // Referencia al collider del zorro (se obtiene dinámicamente)
    private Collider colisionadorZorro;

    private void Awake()
    {
        colisionadorPlataforma = GetComponent<Collider>();

        // Empieza como trigger (atravesable) por defecto
        // CollisionDetector la volverá sólida cuando corresponda
        colisionadorPlataforma.isTrigger = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        // No es necesario aquí, la lógica principal está en CollisionDetector
    }

    /// Comprueba si el zorro viene desde abajo de la plataforma.
    /// Devuelve true si el pie del zorro está por debajo del techo de la plataforma.
    public bool ZorroVieneDesdeAbajo(Transform zorroTransform, float mitadAlturaZorro)
    {
        float pieZorro = zorroTransform.position.y - mitadAlturaZorro;
        float techoPlatforma = colisionadorPlataforma.bounds.max.y;

        return pieZorro < techoPlatforma - margenDeteccion;
    }

    /// Activa el modo fantasma: el zorro puede atravesar la plataforma.
    public void ActivarModoFantasma(Collider zorroCol)
    {
        if (zorroCol != null)
            Physics.IgnoreCollision(colisionadorPlataforma, zorroCol, true);
    }

    /// Desactiva el modo fantasma: la plataforma se vuelve sólida para el zorro.
    public void DesactivarModoFantasma(Collider zorroCol)
    {
        if (zorroCol != null)
            Physics.IgnoreCollision(colisionadorPlataforma, zorroCol, false);
    }

    /// Referencia al collider de la plataforma para uso externo.
    public Collider ColisionadorPlataforma => colisionadorPlataforma;
}