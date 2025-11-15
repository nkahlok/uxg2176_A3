using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameInput gameInput;

    Rigidbody rb;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float smoothTime = 0.3f;
    Vector3 currVelocity;
    Vector3 smoothedVelocity;

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
        if (Player.Instance.playerState == Player.PlayerState.PLAYER_3RD)
        {
            // get player input
            Vector2 input = gameInput.GetMovementVector();

            // get flattened cam vectors and normalise
            Vector3 camForward = CameraManager.Instance.GetCameraForward();
            Vector3 camRight = CameraManager.Instance.GetCameraRight();
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * input.y + camRight * input.x;
            moveDir.Normalize();

            // calculate target velocity
            Vector3 targetVelocity = moveDir * moveSpeed;

            // smooth damp curr velocity
            currVelocity = Vector3.SmoothDamp(currVelocity, targetVelocity, ref smoothedVelocity, smoothTime);

            // update player velocity
            rb.linearVelocity = currVelocity;
        }
        else
        {
            // smooth damp curr velocity
            currVelocity = Vector3.SmoothDamp(currVelocity, Vector3.zero, ref smoothedVelocity, smoothTime);

            // update player velocity
            rb.linearVelocity = currVelocity;
        }
    }
}
