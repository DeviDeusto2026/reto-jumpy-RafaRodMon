using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// GameManager.cs
/// Gestiona el estado global del juego: puntuación, Game Over y reinicio.
/// Necesario para que Camara.cs pueda notificar el fin de partida.
public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }
    public GameObject panelGameOver;
    public TMP_Text textoPuntuacion;
    public TMP_Text textoEnJuego;
    public FoxMovement zorroMovimiento;
    public PlatformSpawning spawner;
    public Camara camaraControlador;
    public Vector3 posicionInicialZorro = Vector3.zero;

    // Puntuación: número de plataformas superadas
    private int puntuacion = 0;
    private float alturaMaximaAlcanzada = 0f;
    private bool juegoActivo = false;
    private float alturaInicial;

    private void Awake()
    {
        // Singleton
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
    }

    private void Start()
    {
        alturaInicial = posicionInicialZorro.y;
        IniciarJuego();
    }

    private void Update()
    {
        if (!juegoActivo) return;

        // Actualiza la puntuación según la altura máxima alcanzada por el zorro
        if (zorroMovimiento != null)
        {
            float alturaActual = zorroMovimiento.transform.position.y;
            if (alturaActual > alturaMaximaAlcanzada)
            {
                alturaMaximaAlcanzada = alturaActual;
                // Puntuación = plataformas aproximadas subidas
                puntuacion = Mathf.FloorToInt((alturaMaximaAlcanzada - alturaInicial) / 2f);
            }
        }

        if (textoEnJuego != null)
            textoEnJuego.text = "Plataformas: " + puntuacion;
    }

    /// Inicia o reinicia el juego.
    public void IniciarJuego()
    {
        juegoActivo = true;
        puntuacion = 0;
        alturaMaximaAlcanzada = alturaInicial;

        if (panelGameOver != null)
            panelGameOver.SetActive(false);

        if (zorroMovimiento != null)
            zorroMovimiento.Resetear(posicionInicialZorro);

        if (camaraControlador != null)
            camaraControlador.ResetearCamara(posicionInicialZorro);

        if (spawner != null)
            spawner.Resetear();
    }

    /// Activa el estado de Game Over.
    /// Llamado por Camara.cs cuando el zorro cae fuera de vista.
    public void ActivarGameOver()
    {
        if (!juegoActivo) return;

        juegoActivo = false;

        if (panelGameOver != null)
            panelGameOver.SetActive(true);

        if (textoPuntuacion != null)
            textoPuntuacion.text = "Plataformas subidas: " + puntuacion;

        Debug.Log("[GameManager] Game Over. Puntuación: " + puntuacion);
    }

    /// Botón de reinicio (asignar al botón de la UI de Game Over).
    public void BotonReiniciar()
    {
        IniciarJuego();
    }
}