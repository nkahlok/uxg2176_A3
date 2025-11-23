using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameInput gameInput;

    Rigidbody rb;
    [SerializeField] float moveSpeed = 5f;

    private void Start()
    {
        gameInput = GetComponent<GameInput>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Player.Instance.playerState == Player.PlayerState.PLAYER)
        {
            // get player input
            Vector2 input = gameInput.GetMovementVector();

            if (input.sqrMagnitude > 0.1f)
            {
                // get flattened cam vectors and normalise
                Vector3 camForward = CameraManager.Instance.GetCameraForward();
                Vector3 camRight = CameraManager.Instance.GetCameraRight();
                camForward.y = 0f;
                camRight.y = 0f;
                camForward.Normalize();
                camRight.Normalize();

                Vector3 moveDir = camForward * input.y + camRight * input.x;
                moveDir.Normalize();

                // update player pos
                rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * moveDir);
            }

            rb.linearVelocity = Vector3.zero;
        }
    }
}
