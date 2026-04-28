using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private Collider platformCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platformCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject fox = GameObject.FindWithTag("Player");

        if (fox != null)
        {
            if (fox.transform.position.y > transform.position.y)
            {
                platformCollider.isTrigger = false;
            }
            else
            {
                platformCollider.isTrigger = true;
            }
        }
    }
}