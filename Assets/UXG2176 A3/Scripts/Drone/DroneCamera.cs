using UnityEngine;

public class DroneCamera : MonoBehaviour
{
    Camera cam;
    Transform parentTransform;
    GameInput gameInput;

    [SerializeField] float distance = 8f;
    [SerializeField] float height = 2f;

    [SerializeField] float vertMinAngle = -40f;
    [SerializeField] float vertMaxAngle = 80f;

    [SerializeField] float rotationSmoothTime = 0.1f;
    [SerializeField] float positionSmoothTime = 0.1f;

    float currYaw = 0f;
    float currPitch = 0f;
    float pitchOffset = 0f;
    float yawVelocity = 0f;
    float pitchVelocity = 0f;
    Vector3 positionVelocity = Vector3.zero;

    private void Start()
    {
        cam = GetComponent<Camera>();

        parentTransform = transform.parent;
        gameInput = parentTransform.GetComponent<GameInput>();

        // position cam at offset
        transform.position += new Vector3(0f, height, -distance);
        transform.LookAt(parentTransform.position);

        // init yaw and pitch
        Vector3 angles = transform.eulerAngles;
        currYaw = angles.y;
        currPitch = angles.x;
        pitchOffset = currPitch;
    }

    private void LateUpdate()
    {
        if (Player.Instance.playerState == Player.PlayerState.DRONE)
        {
            // return if no parent
            if (parentTransform == null)
            {
                return;
            }

            HandleCameraPosition();
        }
    }

    protected void HandleCameraPosition()
    {
        // mouse input
        Vector2 input = gameInput.GetMouseVector().normalized;
        Vector2 sens = gameInput.GetMouseSens();
        input.y *= sens.y;

        // yaw = horizontal rotation around y axis
        // smooth rotation
        CalcCurrYaw();

        // pitch = vertical rotation around x axis
        // smooth pitch
        float targetPitch = currPitch - input.y; // invert for more intuitive feel
        targetPitch = Mathf.Clamp(targetPitch, vertMinAngle - pitchOffset, vertMaxAngle - pitchOffset);
        currPitch = Mathf.SmoothDampAngle(currPitch, targetPitch, ref pitchVelocity, rotationSmoothTime);

        // calculate cam pos based on drone rotation, cam height and distance offset
        Quaternion rotation = Quaternion.Euler(currPitch, currYaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, height, -distance);
        Vector3 targetPos = parentTransform.position + offset;

        // smooth position movement
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref positionVelocity, positionSmoothTime);
        cam.transform.position = transform.position;

        // ensures camera is always facing drone
        transform.LookAt(parentTransform.position);
        cam.transform.LookAt(parentTransform.position);
    }

    public float CalcCurrYaw()
    {
        // mouse input
        Vector2 input = gameInput.GetMouseVector().normalized;
        Vector2 sens = gameInput.GetMouseSens();
        input.x *= sens.x;

        // yaw = horizontal rotation around y axis
        // smooth rotation
        float targetYaw = currYaw + input.x;
        currYaw = Mathf.SmoothDampAngle(currYaw, targetYaw, ref yawVelocity, rotationSmoothTime);

        return currYaw;
    }
}
