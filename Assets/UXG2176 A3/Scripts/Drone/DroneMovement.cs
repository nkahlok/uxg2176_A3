using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    GameInput gameInput;

    Rigidbody rb;
    [SerializeField] float maxHorizontalSpeed = 5f;
    [SerializeField] float maxVerticalSpeed = 5f;

    [SerializeField] float maxTiltAngle = 25f;
    [SerializeField] float tiltSmoothTime = 0.3f;
    Quaternion smoothedRotation;

    [SerializeField] GameObject droneVisual;

    private void Start()
    {
        gameInput = Player.Instance.GetComponent<GameInput>();
        rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        HandleTilt();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Player.Instance.playerState == Player.PlayerState.DRONE)
        {
            // get player input
            Vector3 input = gameInput.GetMovementVector();

            if (input.sqrMagnitude > 0.1f)
            {
                // get flattened cam vectors and normalise
                Vector3 camForward = CameraManager.Instance.GetCameraForward();
                Vector3 camRight = CameraManager.Instance.GetCameraRight();
                camForward.y = 0f;
                camRight.y = 0f;
                camForward.Normalize();
                camRight.Normalize();

                Vector3 moveDir = camForward * input.z + camRight * input.x + Vector3.up * input.y;
                moveDir.Normalize();
                moveDir = new Vector3(moveDir.x * maxHorizontalSpeed, moveDir.y * maxVerticalSpeed, moveDir.z * maxHorizontalSpeed);

                // update player pos
                rb.MovePosition(rb.position + moveDir * Time.fixedDeltaTime);
            }

            rb.linearVelocity = Vector3.zero;
        }
    }

    private void HandleTilt()
    {
        // get input and curr tilt angles
        Vector3 input = gameInput.GetMovementVector();
        Vector3 eulerAngles = droneVisual.transform.eulerAngles;
        float x = 0f;
        float z = 0f;

        if (Player.Instance.playerState == Player.PlayerState.DRONE)
        {
            // tilt drone right (-ve z rotation)
            if (input.x > 0f)
            {
                z = Mathf.SmoothDampAngle(eulerAngles.z, -maxTiltAngle, ref smoothedRotation.z, tiltSmoothTime);
            }
            // tilt drone left (+ve z rotation)
            else if (input.x < 0f)
            {
                z = Mathf.SmoothDampAngle(eulerAngles.z, maxTiltAngle, ref smoothedRotation.z, tiltSmoothTime);
            }
            // tilt drone to normal
            else
            {
                z = Mathf.SmoothDampAngle(eulerAngles.z, 0f, ref smoothedRotation.z, tiltSmoothTime);
            }

            // tilt drone forwards (+ve x rotation)
            if (input.z > 0f)
            {
                x = Mathf.SmoothDampAngle(eulerAngles.x, maxTiltAngle, ref smoothedRotation.x, tiltSmoothTime);
            }
            // tilt drone backwards (-ve x rotation)
            else if (input.z < 0f)
            {
                x = Mathf.SmoothDampAngle(eulerAngles.x, -maxTiltAngle, ref smoothedRotation.x, tiltSmoothTime);
            }
            // tilt drone to normal
            else
            {
                x = Mathf.SmoothDampAngle(eulerAngles.x, 0f, ref smoothedRotation.x, tiltSmoothTime);
            }
        }
        // tilt drone to normal
        else
        {
            x = Mathf.SmoothDampAngle(eulerAngles.x, 0f, ref smoothedRotation.x, tiltSmoothTime);
            z = Mathf.SmoothDampAngle(eulerAngles.z, 0f, ref smoothedRotation.z, tiltSmoothTime);
        }

        droneVisual.transform.rotation = Quaternion.Euler(x, eulerAngles.y, z);
    }
}
