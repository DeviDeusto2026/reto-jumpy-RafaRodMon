using UnityEngine;

public class FoxMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is 
    public int movSpeed = 15;
    public float fSalto = 8f;
    private bool salta;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        move();
        jump();
    }

    private void move()
    {
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.position += Vector3.left * Time.deltaTime * movSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.position += Vector3.right * Time.deltaTime * movSpeed;
        }
    }

    private void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && salta)
        {
            rb.AddForce(Vector3.up * fSalto, ForceMode.Impulse);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            salta = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            salta = true;
        }
    }
}
