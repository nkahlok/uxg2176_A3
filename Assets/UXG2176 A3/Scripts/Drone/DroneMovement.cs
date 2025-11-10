using UnityEngine;
using UnityEngine.Windows;

public class DroneMovement : MonoBehaviour
{
    GameInput gameInput;

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
    Quaternion currRotation;
    Quaternion smoothedRotation;

    [SerializeField] bool debugCasts = false;

    private void Start()
    {
        gameInput = GetComponent<GameInput>();
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

        // prevent drone from flying through the floor
        //RaycastHit hit;
        //if (Physics.SphereCast(transform.position, sphereCastRadius, Vector3.down, out hit, minHeightFromGround, LayerMask.GetMask("Default")))
        //{
        //    if (currVelocity.y < 0f)
        //    {
        //        currVelocity = new Vector3(currVelocity.x, 0f, currVelocity.z);
        //    }
        //}

        //if (Physics.SphereCast(transform.position, sphereCastRadius, moveDir, out hit, minDistFromObstacles, LayerMask.GetMask("Default")) || Physics.SphereCast(transform.position, sphereCastRadius, currVelocity, out hit, minDistFromObstacles, LayerMask.GetMask("Default")))
        //{
        //    currVelocity = Vector3.zero;
        //}

        // update position with calculated velocity
        transform.position += currVelocity * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (debugCasts)
        {
            Vector3 input = gameInput.GetMovementVector();
            Vector3 camForward = droneCamera.transform.forward;
            Vector3 camRight = droneCamera.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * input.z + camRight * input.x;
            moveDir.Normalize();

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sphereCastRadius);

            RaycastHit hit;
            if (Physics.SphereCast(transform.position, sphereCastRadius, moveDir, out hit, minDistFromObstacles, LayerMask.GetMask("Default")))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, hit.point);
                Gizmos.DrawSphere(hit.point, sphereCastRadius);
            }
            else if (Physics.SphereCast(transform.position, sphereCastRadius, currVelocity, out hit, minDistFromObstacles, LayerMask.GetMask("Default")))
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, hit.point);
                Gizmos.DrawSphere(hit.point, sphereCastRadius);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + moveDir * minDistFromObstacles);
                Gizmos.DrawWireSphere(transform.position + moveDir * minDistFromObstacles, sphereCastRadius);
            }
        }
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
