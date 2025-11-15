using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    GameInput gameInput;

    Rigidbody rb;
    [SerializeField] float maxHorizontalSpeed = 5f;
    [SerializeField] float maxVerticalSpeed = 5f;
    [SerializeField] float smoothTime = 0.3f;
    Vector3 currVelocity;
    Vector3 smoothedVelocity;

    [SerializeField] float maxTiltAngle = 25f;
    [SerializeField] float tiltSmoothTime = 0.3f;
    Quaternion smoothedRotation;

    private void Start()
    {
        gameInput = GetComponent<GameInput>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
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

            // get flattened cam vectors and normalise
            Vector3 camForward = CameraManager.Instance.GetCameraForward();
            Vector3 camRight = CameraManager.Instance.GetCameraRight();
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

            // update drone velocity
            rb.linearVelocity = currVelocity;
        }
        else
        {
            // smooth damp curr velocity
            currVelocity = Vector3.SmoothDamp(currVelocity, Vector3.zero, ref smoothedVelocity, smoothTime);

            // update drone velocity
            rb.linearVelocity = currVelocity;
        }
    }

    private void HandleTilt()
    {
        // get input and curr tilt angles
        Vector3 input = gameInput.GetMovementVector();
        Vector3 eulerAngles = transform.GetChild(0).eulerAngles;
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

            transform.GetChild(0).rotation = Quaternion.Euler(x, CameraManager.Instance.currYaw, z);
        }
        // tilt drone to normal
        else
        {
            x = Mathf.SmoothDampAngle(eulerAngles.x, 0f, ref smoothedRotation.x, tiltSmoothTime);
            z = Mathf.SmoothDampAngle(eulerAngles.z, 0f, ref smoothedRotation.z, tiltSmoothTime);

            transform.GetChild(0).rotation = Quaternion.Euler(x, eulerAngles.y, z);
        }
    }
}
