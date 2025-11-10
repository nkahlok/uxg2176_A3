using UnityEngine;
using UnityEngine.Windows;

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
    [SerializeField] float maxTiltAngle = 15f;
    [SerializeField] float tiltSmoothTime = 0.3f;
    Quaternion currRotation;
    Quaternion smoothedRotation;

    private void Start()
    {
        gameInput = GetComponent<GameInput>();
        rb = GetComponent<Rigidbody>();
        Player.Instance.SwitchMode(Player.PlayerState.DRONE);

        currRotation = transform.rotation;
    }

    private void Update()
    {
        if (Player.Instance.playerState == Player.PlayerState.DRONE)
        {
            HandleMovement();
            //HandleRotation();
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
        transform.position += currVelocity * Time.deltaTime;
    }

    private void HandleRotationNew()
    {
        Vector3 input = gameInput.GetMovementVector();
        if (input.x > 0f)
        {

        }
        else if (input.x < 0f)
        {

        }

        if (input.z > 0f)
        {

        }
        else if (input.z < 0f)
        {

        }
        
        //float x = Mathf.SmoothDampAngle()
    }

    private void HandleRotation()
    {
        // get flattened cam vectors and normalise
        Vector3 camForward = droneCamera.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 horizontalVelocity = new Vector3(currVelocity.x, 0f, currVelocity.z);
        Quaternion yawRotation = Quaternion.LookRotation(camForward);

        float tiltX = 0f;
        float tiltZ = 0f;

        if (horizontalVelocity.magnitude > 0f)
        {
            Vector3 localVelocity = Quaternion.Inverse(yawRotation) * horizontalVelocity;

            // calc tilt based on local velocity
            tiltX = -Mathf.Clamp(localVelocity.z / maxHorizontalSpeed, -1f, 1f) * maxTiltAngle;
            tiltZ = -Mathf.Clamp(localVelocity.x / maxHorizontalSpeed, -1f, 1f) * maxTiltAngle;
        }

        Quaternion tiltRotation = Quaternion.Euler(tiltX, 0f, tiltZ);
        Quaternion targetRotation = yawRotation * tiltRotation;

        //currRotation = SmoothDampQuaternion(currRotation, targetRotation, ref smoothedRotation, tiltSmoothTime);
        //transform.rotation = currRotation;
    }

    private Quaternion SmoothDampQuaternion(Quaternion curr, Quaternion target, ref Quaternion velocity, float smoothTime)
    {
        Vector3 currEuler = curr.eulerAngles;
        Vector3 targetEuler = target.eulerAngles;
        Vector3 velocityEuler = velocity.eulerAngles;

        float x = Mathf.SmoothDampAngle(currEuler.x, targetEuler.x, ref velocityEuler.x, smoothTime);
        float y = Mathf.SmoothDampAngle(currEuler.y, targetEuler.y, ref velocityEuler.y, smoothTime);
        float z = Mathf.SmoothDampAngle(currEuler.z, targetEuler.z, ref velocityEuler.z, smoothTime);

        velocity = new Quaternion(velocityEuler.x, velocityEuler.y, velocityEuler.z, 0f);
        return Quaternion.Euler(x, y, z);
    }
}
