using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    GameInput gameInput;
    Rigidbody rb;
    [SerializeField] Vector3 movementForce = Vector3.zero;
    [SerializeField] float maxHorizontalSpeed = 20f;
    [SerializeField] float maxVerticalSpeed = 10f;
    [SerializeField] float rotationSpeed = 10f;

    [SerializeField] Transform camTransform;

    private void Start()
    {
        gameInput = GetComponent<GameInput>();
        rb = GetComponent<Rigidbody>();
        Player.Instance.SwitchMode(Player.PlayerState.DRONE);
    }

    private void FixedUpdate()
    {
        if (Player.Instance.playerState == Player.PlayerState.DRONE)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        // get player input
        Vector3 input = gameInput.GetMovementVector();

        // add appropriate force in the direction of player input
        if (input != Vector3.zero)
        {
            // calculates force to add
            Vector3 force = new Vector3 (movementForce.x * input.x, movementForce.y * input.y, movementForce.z * input.z);
            rb.AddForce(force);

            // clamps horizontal and vertical speeds
            Vector3 velocity = rb.linearVelocity;
            Vector2 horizontal = new Vector2(velocity.x, velocity.z);
            if (horizontal.magnitude > maxHorizontalSpeed)
            {
                horizontal = horizontal.normalized * maxHorizontalSpeed;
                velocity.x = horizontal.x;
                velocity.z = horizontal.y;
            }
            float vertical = velocity.y;
            if (vertical > maxVerticalSpeed)
            {
                vertical = maxVerticalSpeed;
                velocity.y = vertical;
            }
            rb.linearVelocity = velocity;
        }
    }
}
