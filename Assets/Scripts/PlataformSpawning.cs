using System.Collections.Generic;
using UnityEngine;

/// PlatformSpawning.cs
/// Genera plataformas de manera aleatoria conforme el zorro sube.
/// - Las plataformas aparecen en posiciones X e Y aleatorias dentro del área de spawneo.
/// - La diferencia de Y entre plataformas no supera lo que el zorro puede saltar.
/// - Las plataformas fuera de vista (muy por debajo de la cámara) se eliminan o reciclan.
public class PlatformSpawning : MonoBehaviour
{
    public GameObject prefabPlataforma;
    public float anchoAreaSpawneo = 10f;
    public float alturaMinEntreNiveles = 1.5f;
    public float alturaMaxEntreNiveles = 3.5f;
    public float factorAreaSpawneo = 1f; ///de 0,1 a 3 para evitar que el área se vuelva demasiado pequeña o grande
    public int plataformasPorAdelante = 2;
    public float distanciaEliminacion = 15f;
    public Transform zorro;
    public Transform camara;

    // Posición Y de la última plataforma generada
    private float ultimaYGenerada;

    // Lista de plataformas activas en escena
    private List<GameObject> plataformasActivas = new List<GameObject>();

    // Posición X del zorro (para centrar el área de spawneo)
    private float centroX;

    private void Start()
    {
        if (prefabPlataforma == null)
        {
            Debug.LogError("[PlatformSpawning] No se ha asignado el prefab de plataforma.");
            return;
        }

        centroX = zorro != null ? zorro.position.x : 0f;

        // Genera la plataforma inicial justo debajo del zorro
        ultimaYGenerada = zorro != null ? zorro.position.y - 1f : 0f;

        // Pre-genera las primeras plataformas
        GenerarPlataformasIniciales();
    }

    private void Update()
    {
        if (zorro == null) return;

        // Genera más plataformas si el zorro se acerca al límite generado
        float alturaObjetivo = zorro.position.y + (plataformasPorAdelante * alturaMaxEntreNiveles);
        while (ultimaYGenerada < alturaObjetivo)
        {
            GenerarSiguientePlataforma();
        }

        // Elimina plataformas que ya no son visibles (por debajo de la cámara)
        EliminarPlataformasViejas();
    }

    /// Genera las plataformas de inicio (suelo + primeras plataformas).
    private void GenerarPlataformasIniciales()
    {
        for (int i = 0; i < plataformasPorAdelante; i++)
        {
            GenerarSiguientePlataforma();
        }
    }

    /// Genera una nueva plataforma en una posición aleatoria válida.
    private void GenerarSiguientePlataforma()
    {
        float nuevoY = ultimaYGenerada + Random.Range(alturaMinEntreNiveles, alturaMaxEntreNiveles);

        // Ancho real del área de spawneo (modificado por el factor)
        float anchoReal = anchoAreaSpawneo * factorAreaSpawneo;
        float mitadAncho = anchoReal / 2f;
        float nuevoX = Random.Range(centroX - mitadAncho, centroX + mitadAncho);

        Vector3 posicion = new Vector3(nuevoX, nuevoY, 0f);
        GameObject nuevaPlataforma = Instantiate(prefabPlataforma, posicion, Quaternion.identity);
        nuevaPlataforma.tag = "Platform";

        plataformasActivas.Add(nuevaPlataforma);
        ultimaYGenerada = nuevoY;
    }

    /// Elimina plataformas que están muy por debajo de la cámara.
    private void EliminarPlataformasViejas()
    {
        if (camara == null) return;

        float limiteInferior = camara.position.y - distanciaEliminacion;

        for (int i = plataformasActivas.Count - 1; i >= 0; i--)
        {
            if (plataformasActivas[i] == null)
            {
                plataformasActivas.RemoveAt(i);
                continue;
            }

            if (plataformasActivas[i].transform.position.y < limiteInferior)
            {
                Destroy(plataformasActivas[i]);
                plataformasActivas.RemoveAt(i);
            }
        }
    }

    /// Resetea el spawner al inicio de una nueva partida.
    public void Resetear()
    {
        // Destruye todas las plataformas activas
        foreach (GameObject p in plataformasActivas)
        {
            if (p != null) Destroy(p);
        }
        plataformasActivas.Clear();

        // Reinicia desde la posición del zorro
        ultimaYGenerada = zorro != null ? zorro.position.y - 1f : 0f;
        GenerarPlataformasIniciales();
    }

    /// Devuelve la cantidad de plataformas generadas hasta ahora (para puntuación).
    public int CantidadPlataformasGeneradas => plataformasActivas.Count;

    private void OnDrawGizmosSelected()
    {
        // Visualiza el área de spawneo en el editor
        float anchoReal = anchoAreaSpawneo * factorAreaSpawneo;
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.3f);
        Gizmos.DrawCube(
            new Vector3(transform.position.x, ultimaYGenerada, 0f),
            new Vector3(anchoReal, 1f, 0.5f)
        );
    }
}