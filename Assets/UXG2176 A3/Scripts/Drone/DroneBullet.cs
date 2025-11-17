using UnityEngine;

public class DroneBullet : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float moveSpeed;
    Vector3 moveDir;
    [SerializeField] float lifetime;
    float lifetimeTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lifetimeTimer = lifetime;
        moveDir = transform.forward;
    }

    private void Update()
    {
        if (lifetimeTimer > 0f)
        {
            lifetimeTimer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        rb.MovePosition(rb.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
