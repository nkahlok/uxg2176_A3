using UnityEngine;

public class DroneCamera : MonoBehaviour
{
    Camera droneCamera;
    Transform droneTransform;
    GameInput gameInput;

    [SerializeField] float distance = 8f;
    [SerializeField] float height = 2f;

    [SerializeField] float horizMouseSens = 3f;
    [SerializeField] float vertMouseSens = 3f;
    [SerializeField] float vertMinAngle = -40f;
    [SerializeField] float vertMaxAngle = 80f;

    [SerializeField] float rotationSmoothTime = 0.1f;
    [SerializeField] float positionSmoothTime = 0.1f;

    float currYaw = 0f;
    float currPitch = 0f;
    float yawVelocity = 0f;
    float pitchVelocity = 0f;
    Vector3 positionVelocity = Vector3.zero;

    private void Start()
    {
        droneCamera = Camera.main;
        droneTransform = transform.parent;
        gameInput = droneTransform.GetComponent<GameInput>();

        // init yaw and pitch
        Vector3 angles = transform.eulerAngles;
        currYaw = angles.y;
        currPitch = angles.x;
    }

    private void FixedUpdate()
    {
        if (Player.Instance.playerState == Player.PlayerState.DRONE)
        {
            // return if no drone
            if (droneTransform == null)
            {
                return;
            }

            HandleDroneRotation();
            HandleCameraPosition();
        }
    }

    private void HandleDroneRotation()
    {
        Vector2 input = gameInput.GetMouseVector().normalized;
        input.x *= horizMouseSens;
        input.y *= vertMouseSens;

        // yaw = horizontal rotation around y axis
        // pitch = vertical rotation around x axis
        float targetYaw = currYaw + input.x;
        float targetPitch = currPitch - input.y; // invert for more intuitive feel
        targetPitch = Mathf.Clamp(targetPitch, vertMinAngle, vertMaxAngle);

        // smooth rotation
        currYaw = Mathf.SmoothDampAngle(currYaw, targetYaw, ref yawVelocity, rotationSmoothTime);
        currPitch = Mathf.SmoothDampAngle(currPitch, targetPitch, ref pitchVelocity, rotationSmoothTime);

        // apply yaw to drone rotation
        droneTransform.rotation = Quaternion.Euler(0f, currYaw, 0f);
    }

    private void HandleCameraPosition()
    {
        // calculate cam pos based on drone rotation, cam height and distance offset
        Quaternion rotation = Quaternion.Euler(currPitch, currYaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, height, -distance);
        Vector3 targetPos = droneTransform.position + offset;

        // smooth position movement
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref positionVelocity, positionSmoothTime);
        droneCamera.transform.position = transform.position;

        // ensures camera is always facing drone
        transform.LookAt(droneTransform.position);
        droneCamera.transform.LookAt(droneTransform.position);
    }
}
