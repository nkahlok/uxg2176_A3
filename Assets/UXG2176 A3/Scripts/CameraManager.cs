using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [Header("Cinemachine Virtual Cameras")]
    CinemachineCamera activeCam;
    [SerializeField] CinemachineCamera playerVirtualCam;
    [SerializeField] CinemachineCamera droneVirtualCam;
    [SerializeField] CinemachineCamera[] cctvVirtualCams;
    int cctvVirtualCamIndex = 0;

    [Space(10)]
    [Header("Rotation Settings")]
    [SerializeField] float rotationSmoothTime = 0.3f;
    [SerializeField] float minPitch = -20f;
    [SerializeField] float maxPitch = 60f;

    GameInput gameInput;
    public float currYaw { get; private set; } = 0f;
    public float currPitch { get; private set; } = 0f;
    float yawVelocity = 0f;
    float pitchVelocity = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameInput = Player.Instance.GetComponent<GameInput>();
    }

    private void LateUpdate()
    {
        // rotate the camera if 3rd person view
        if (Player.Instance.playerState == Player.PlayerState.PLAYER_3RD ||
            Player.Instance.playerState == Player.PlayerState.DRONE)
        {
            HandleCameraRotation();
        }
    }

    private void InitialiseCamera()
    {
        // init curr yaw and curr pitch based on curr active cam
        activeCam = GetActiveCamera();
        if (activeCam != null)
        {
            Vector3 angles = activeCam.transform.eulerAngles;
            currYaw = angles.y;
            currPitch = angles.x;

            // reset velocities
            yawVelocity = 0f;
            pitchVelocity = 0f;
        }
    }

    public CinemachineCamera GetActiveCamera()
    {
        // switch active cam based on player state
        switch (Player.Instance.playerState)
        {
            case Player.PlayerState.PLAYER_1ST:
                return null;

            case Player.PlayerState.PLAYER_3RD:
                return playerVirtualCam;

            case Player.PlayerState.DRONE:
                return droneVirtualCam;

            case Player.PlayerState.CCTV:
                return cctvVirtualCams[cctvVirtualCamIndex];

            default:
                return null;
        }
    }

    public Vector3 GetCameraForward()
    {
        if (activeCam == null || activeCam.Follow == null)
        {
            return Vector3.forward;
        }

        Vector3 forward = activeCam.Follow.forward;
        forward.y = 0f;
        forward.Normalize();

        return forward;
    }

    public Vector3 GetCameraRight()
    {
        if (activeCam == null || activeCam.Follow == null)
        {
            return Vector3.right;
        }

        Vector3 right = activeCam.Follow.right;
        right.y = 0f;
        right.Normalize();

        return right;
    }

    public void SetCameraMode(int cctvVirtualCamIndex = 0)
    {
        // reset all virtual cam priority
        playerVirtualCam.Priority.Value = 0;
        droneVirtualCam.Priority.Value = 0;
        foreach (CinemachineCamera cam in cctvVirtualCams)
        {
            cam.Priority.Value = 0;
        }
        this.cctvVirtualCamIndex = cctvVirtualCamIndex;

        // update active cam priority to highest
        switch (Player.Instance.playerState)
        {
            case Player.PlayerState.PLAYER_1ST:
                break;

            case Player.PlayerState.PLAYER_3RD:
                playerVirtualCam.Priority.Value = 10;
                InitialiseCamera();
                break;

            case Player.PlayerState.DRONE:
                droneVirtualCam.Priority.Value = 10;
                InitialiseCamera();
                break;

            case Player.PlayerState.CCTV:
                cctvVirtualCams[cctvVirtualCamIndex].Priority.Value = 10;
                InitialiseCamera();
                break;

            default:
                break;
        }
    }

    private void HandleCameraRotation()
    {
        // get mouse values
        Vector2 mouseInput = gameInput.GetMouseVector().normalized;
        Vector2 mouseSens = gameInput.GetMouseSens();

        // calculate target yaw and pitch
        float targetYaw = currYaw + mouseInput.x * mouseSens.x;
        float targetPitch = currPitch - mouseInput.y * mouseSens.y;
        targetPitch = Mathf.Clamp(targetPitch, minPitch, maxPitch);

        // gradually update curr yaw and pitch
        currYaw = Mathf.SmoothDampAngle(currYaw, targetYaw, ref yawVelocity, rotationSmoothTime * Time.fixedDeltaTime);
        currPitch = Mathf.SmoothDampAngle(currPitch, targetPitch, ref pitchVelocity, rotationSmoothTime * Time.fixedDeltaTime);

        // update active cam tracked target
        activeCam.Follow.rotation = Quaternion.Euler(currPitch, currYaw, 0f);
    }
}
