using UnityEngine;

public class Winning : MonoBehaviour
{
    [SerializeField] private GameObject winText;

    private const string PLAYER_NAME = "Character";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != PLAYER_NAME) return;

        if (winText != null)
            winText.SetActive(true);

        Time.timeScale = 0f;
    }
}