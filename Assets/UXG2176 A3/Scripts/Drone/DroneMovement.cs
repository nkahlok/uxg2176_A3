using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    GameInput gameInput;
    Rigidbody rb;

    [SerializeField] float maxHorizontalSpeed = 20f;
    [SerializeField] float maxVerticalSpeed = 10f;
    [SerializeField] float smoothTime = 0.3f;
    Vector3 currVelocity;
    Vector3 smoothedVelocity;

    [SerializeField] DroneCamera droneCamera;

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

        // get cam vectors and flatten
        Vector3 camForward = droneCamera.transform.forward;
        Vector3 camRight = droneCamera.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * input.z + camRight * input.x;
        moveDir.Normalize();

        // calculate target velocity
        Vector3 targetVelocity = moveDir * maxHorizontalSpeed;
        targetVelocity.y = input.y * maxVerticalSpeed;

        // smooth damp curr velocity
        currVelocity = Vector3.SmoothDamp(currVelocity, targetVelocity, ref smoothedVelocity, smoothTime);
        rb.linearVelocity = currVelocity;
    }
}
