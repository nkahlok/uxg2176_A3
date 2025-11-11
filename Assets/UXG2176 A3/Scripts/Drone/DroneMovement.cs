using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    GameInput gameInput;

    Rigidbody rb;
    [SerializeField] float maxHorizontalSpeed = 20f;
    [SerializeField] float maxVerticalSpeed = 10f;
    [SerializeField] float smoothTime = 0.3f;
    [SerializeField] float sphereCastRadius = 0.5f;
    [SerializeField] float minDistFromObstacles = 1f;
    [SerializeField] float minHeightFromGround = 0.3f;
    Vector3 currVelocity;
    Vector3 smoothedVelocity;

    [SerializeField] DroneCamera droneCamera;
    [SerializeField] float maxTiltAngle = 15f;
    [SerializeField] float tiltSmoothTime = 0.3f;
    Quaternion smoothedRotation;

    [SerializeField] bool debugCasts = false;

    private void Start()
    {
        gameInput = GetComponent<GameInput>();
        rb = GetComponent<Rigidbody>();
        Player.Instance.SwitchMode(Player.PlayerState.DRONE);
    }

    private void Update()
    {
        if (Player.Instance.playerState == Player.PlayerState.DRONE)
        {
            HandleMovement();
            HandleRotation();
        }
    }

    private void HandleMovement()
    {
        // get player input
        Vector3 input = gameInput.GetMovementVector();

        // get flattened cam vectors and normalise
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

        // update position with calculated velocity
        rb.linearVelocity = currVelocity;
    }

    private void HandleRotation()
    {
        Vector3 input = gameInput.GetMovementVector();
        Vector3 eulerAngles = transform.GetChild(0).eulerAngles;
        float x = 0f;
        float z = 0f;

        if (input.x > 0f)
        {
            z = Mathf.SmoothDampAngle(eulerAngles.z, -maxTiltAngle, ref smoothedRotation.z, tiltSmoothTime);
        }
        else if (input.x < 0f)
        {
            z = Mathf.SmoothDampAngle(eulerAngles.z, maxTiltAngle, ref smoothedRotation.z, tiltSmoothTime);
        }
        else
        {
            z = Mathf.SmoothDampAngle(eulerAngles.z, 0f, ref smoothedRotation.z, tiltSmoothTime);
        }

        if (input.z > 0f)
        {
            x = Mathf.SmoothDampAngle(eulerAngles.x, maxTiltAngle, ref smoothedRotation.x, tiltSmoothTime);
        }
        else if (input.z < 0f)
        {
            x = Mathf.SmoothDampAngle(eulerAngles.x, -maxTiltAngle, ref smoothedRotation.x, tiltSmoothTime);
        }
        else
        {
            x = Mathf.SmoothDampAngle(eulerAngles.x, 0f, ref smoothedRotation.x, tiltSmoothTime);
        }

        transform.GetChild(0).rotation = Quaternion.Euler(x, droneCamera.CalcCurrYaw(), z);
    }
}
