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

    private void Start()
    {
        Instance = this;

        gameInput = Player.Instance.GetComponent<GameInput>();
    }

    private void LateUpdate()
    {
        if (activeCam != null)
        {
            switch (Player.Instance.playerState)
            {
                // rotate the camera if not fixed angle
                case Player.PlayerState.PLAYER:
                case Player.PlayerState.DRONE:
                    HandleCameraRotation();
                    break;

                case Player.PlayerState.CCTV:
                default:
                    break;
            }
        }
    }

    private void InitialiseCamera()
    {
        // switch active cam based on player state
        switch (Player.Instance.playerState)
        {
            case Player.PlayerState.PLAYER:
                activeCam = playerVirtualCam;
                break;

            case Player.PlayerState.DRONE:
                activeCam = droneVirtualCam;
                break;

            case Player.PlayerState.CCTV:
                activeCam = cctvVirtualCams[cctvVirtualCamIndex];
                break;

            default:
                break;
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

     public int GetCCTVCameraCount()
    {
        if (cctvVirtualCams == null)
        {
            return 0;
        }
        return cctvVirtualCams.Length;
    }


    public void SetCameraMode(int cctvVirtualCamIndex = 0)
    {
        // reset all virtual cam priority
        playerVirtualCam.Priority.Value = 0;
        if (droneVirtualCam != null) droneVirtualCam.Priority.Value = 0;
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
                if (cctvVirtualCams != null && cctvVirtualCams.Length > 0 && cctvVirtualCamIndex < cctvVirtualCams.Length)
                {
                    cctvVirtualCams[cctvVirtualCamIndex].Priority.Value = 10;
                    InitialiseCamera();
                }
                else
                {
                    Debug.LogWarning("CCTV cameras not set up or index out of range!");
                }
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

        activeCam.Follow.parent.Rotate(Vector3.up, yawDelta);
        activeCam.Follow.Rotate(Vector3.right, pitchDelta);

        // clamp pitch
        Vector3 angles = activeCam.Follow.eulerAngles;
        if (angles.x > 180f)
        {
            angles.x -= 360f;
        }

        angles.x = Mathf.Clamp(angles.x, -maxPitch, -minPitch);
        angles.z = 0f;
        activeCam.Follow.rotation = Quaternion.Euler(angles);
    }
}
