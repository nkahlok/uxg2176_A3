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
    [SerializeField] float horizontalRotationSpeed = 80f;
    [SerializeField] float verticalRotationSpeed = 120f;
    [SerializeField] float minPitch = -20f;
    [SerializeField] float maxPitch = 60f;

    GameInput gameInput;

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
        if (Player.Instance.playerState == Player.PlayerState.PLAYER ||
            Player.Instance.playerState == Player.PlayerState.DRONE)
        {
            HandleCameraRotation();
        }
    }

    private void InitialiseCamera()
    {
        // set active cam
        activeCam = GetActiveCamera();
    }

    private CinemachineCamera GetActiveCamera()
    {
        // switch active cam based on player state
        switch (Player.Instance.playerState)
        {
            case Player.PlayerState.PLAYER:
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
            case Player.PlayerState.PLAYER:
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
        Vector2 mouseInput = gameInput.GetMouseVector();

        float yawDelta = mouseInput.x * horizontalRotationSpeed * Time.deltaTime;
        float pitchDelta = -mouseInput.y * verticalRotationSpeed * Time.deltaTime;
        
        activeCam.Follow.Rotate(Vector3.right, pitchDelta);
        activeCam.Follow.parent.Rotate(Vector3.up, yawDelta);
    }
}
