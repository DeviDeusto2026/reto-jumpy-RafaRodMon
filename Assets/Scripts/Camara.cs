using UnityEngine;

public class Camara : MonoBehaviour
{
    public Transform zorro;
    public float velocidadSuavizado = 5f;
    public float offsetY = 3f;
    public float umbralSubida = 2f;
    public float limiteInferiorOffset = 1f;

    // Altura mínima que ha alcanzado la cámara (no desciende)
    private float alturaMaximaCamara;
    // Referencia al GameManager para notificar Game Over
    private GameManager gameManager;
    // Posición fija en X de la cámara
    private float posicionX;
    // Posición fija en Z de la cámara
    private float posicionZ;

    private void Start()
    {
        if (zorro == null)
        {
            Debug.LogError("[Camara] No se ha asignado el Transform del zorro.");
            return;
        }

        // La cámara mantiene su posición X y Z fija para simular 2D
        posicionX = transform.position.x;
        posicionZ = transform.position.z;

        // Altura inicial
        alturaMaximaCamara = transform.position.y;

        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
            Debug.LogWarning("[Camara] No se encontró GameManager en la escena.");
    }

    private void LateUpdate()
    {
        if (zorro == null) return;

        ActualizarPosicionCamara();
        ComprobarGameOver();
    }

    /// Mueve la cámara verticalmente siguiendo al zorro, pero solo hacia arriba.
    private void ActualizarPosicionCamara()
    {
        float objetivoY = zorro.position.y + offsetY;

        // La cámara solo asciende: si el objetivo está por encima de la altura máxima, actualiza
        if (objetivoY > alturaMaximaCamara)
        {
            alturaMaximaCamara = Mathf.Lerp(alturaMaximaCamara, objetivoY, velocidadSuavizado * Time.deltaTime);
        }

        // Aplica posición: X y Z fijos, Y animada
        transform.position = new Vector3(posicionX, alturaMaximaCamara, posicionZ);
    }

    /// Comprueba si el zorro ha caído por debajo del borde inferior de la cámara.
    private void ComprobarGameOver()
    {
        // Calcula el borde inferior visible de la cámara
        Camera cam = Camera.main;
        float mitadAlturaMundo = 0f;

        if (cam != null && cam.orthographic)
        {
            mitadAlturaMundo = cam.orthographicSize;
        }
        else if (cam != null)
        {
            // Para cámara en perspectiva, estimamos el borde inferior
            float distancia = Mathf.Abs(transform.position.z - zorro.position.z);
            mitadAlturaMundo = distancia * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        }

        float bordeInferior = transform.position.y - mitadAlturaMundo - limiteInferiorOffset;

        if (zorro.position.y < bordeInferior)
        {
            if (gameManager != null)
                gameManager.ActivarGameOver();
        }
    }

    /// Resetea la cámara a su posición inicial (llamado al reiniciar la partida).
    public void ResetearCamara(Vector3 posicionInicial)
    {
        alturaMaximaCamara = posicionInicial.y + offsetY;
        transform.position = new Vector3(posicionX, alturaMaximaCamara, posicionZ);
    }
}