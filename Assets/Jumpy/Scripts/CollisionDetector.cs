using UnityEngine;

public class PlatformPassThrough : MonoBehaviour
{
    private Collider playerCollider;

    void Start()
    {
        playerCollider = GetComponent<Collider>();
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Platform")) return;

        float platformTop = other.bounds.max.y;
        float playerBottom = playerCollider.bounds.min.y;

        other.isTrigger = playerBottom < platformTop;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Platform")) return;

        // Al salir, nos aseguramos de que quede sólida
        other.isTrigger = false;
    }
}