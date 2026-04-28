using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Camera : MonoBehaviour
{

    public float cameraSpeed = 1f;
    [SerializeField] private Transform fox;
    private Camera cam;
    private float maxReachedHeight;

    void Start()
    {
        cam = GetComponent<Camera>();
        maxReachedHeight = transform.position.y;
    }

    void LateUpdate()
    {
        if (fox == null) return;

        maxReachedHeight += cameraSpeed * Time.deltaTime;

        float targetHeight = fox.position.y;
        if (targetHeight > maxReachedHeight)
        {
            maxReachedHeight = targetHeight;
        }

        transform.position = new Vector3(
            transform.position.x,
            maxReachedHeight,
            transform.position.z
        );

        Vector3 viewportPos = cam.WorldToViewportPoint(fox.position);

        if (viewportPos.y < 0f)
        {
            Debug.Log("¡El zorro ha salido de la vista por abajo!");
            ReiniciarJuego();
        }
    }

    private Vector3 WorldToViewportPoint(Vector3 position)
    {
        throw new NotImplementedException();
    }

    void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}