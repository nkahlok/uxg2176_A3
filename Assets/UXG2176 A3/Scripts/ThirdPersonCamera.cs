using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    Camera cam;
    Transform targetTransform;
    GameInput gameInput;

    [SerializeField] float distance = 8f;
    [SerializeField] float height = 2f;

    [SerializeField] float camColliderRadius = 0.5f;
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

        targetTransform = transform.parent;
        gameInput = targetTransform.GetComponent<GameInput>();
    }

    private void LateUpdate()
    {
        if (cam.enabled)
        {
            // return if no parent
            if (targetTransform == null)
            {
                return;
            }

            HandleCameraPosition();
        }
    }

    public void InitCameraPosition()
    {
        // calculate cam pos based on parent rotation, cam height and distance offset
        Vector3 offset = new Vector3(0f, height, -distance);
        transform.position = CheckIfCamBlocked(offset);

        // ensures camera is always facing parent
        transform.LookAt(targetTransform.position);

        // init yaw and pitch
        Vector3 angles = transform.eulerAngles;
        currYaw = angles.y;
        currPitch = angles.x;
        pitchOffset = currPitch;
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
        targetPitch = Mathf.Clamp(targetPitch, vertMinAngle, vertMaxAngle);
        currPitch = Mathf.SmoothDampAngle(currPitch, targetPitch, ref pitchVelocity, rotationSmoothTime);

        // calculate cam pos based on parent rotation, cam height and distance offset
        Quaternion rotation = Quaternion.Euler(currPitch - pitchOffset, currYaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, height, -distance);
        Vector3 targetPos = CheckIfCamBlocked(offset);

        // smooth position movement
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref positionVelocity, positionSmoothTime);

        // ensures camera is always facing parent
        transform.LookAt(targetTransform.position);
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

    private Vector3 CheckIfCamBlocked(Vector3 camDir)
    {
        // spherecast in direction of camera
        Ray ray = new Ray(targetTransform.position, camDir);
        if (Physics.SphereCast(ray, camColliderRadius, out RaycastHit hit, camDir.magnitude))
        {
            // if obstacle detected, shift camera to be a small distance away from the hit point
            float camDist = hit.distance - camColliderRadius * 0.1f;

            // calculate cam pos
            return targetTransform.position + camDir.normalized * camDist;
        }

        // if no obstacles detected, shift camera to original intended pos
        return targetTransform.position + camDir;
    }
}
