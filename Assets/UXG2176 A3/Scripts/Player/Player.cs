using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    GameInput gameInput;

    public enum PlayerState
    {
        PLAYER,
        CCTV,
        DRONE,

        KEYPAD,

        TRANSITION = 100,
    };

    [HideInInspector] public PlayerState playerState = PlayerState.TRANSITION;

    [SerializeField] float spawnDelayDuration = 2f;
    float spawnDelayTimer = 0f;
    bool hasSpawned = false;
    private int currentCCTVIndex = 0;

    [SerializeField] Canvas playerCanvas;
    [SerializeField] Canvas droneCanvas;
    [SerializeField] Canvas keypadCanvas;

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
        gameInput = GetComponent<GameInput>();

        spawnDelayTimer = spawnDelayDuration;

        // disable all UI except player
        droneCanvas.enabled = false;
    }

    private void Update()
    {
        // spawn delay
        if (hasSpawned)
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                SwitchMode(PlayerState.PLAYER);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                if (playerState == PlayerState.CCTV)
                {
                    CycleCCTVCamera();
                }
                else
                {
                    currentCCTVIndex = 0;
                    SwitchMode(PlayerState.CCTV);
                }
            }
            else if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                SwitchMode(PlayerState.DRONE);
            }
        }
        else
        {
            if (spawnDelayTimer > 0f)
            {
                spawnDelayTimer -= Time.deltaTime;
            }
            else
            {
                hasSpawned = true;
                SwitchMode(PlayerState.PLAYER);
            }
        }
    }

    private void CycleCCTVCamera()
    {
        currentCCTVIndex++;

        int numCCTVCameras = CameraManager.Instance.GetCCTVCameraCount();
        
        if (currentCCTVIndex >= numCCTVCameras)
        {
            currentCCTVIndex = 0;
        }
        
        CameraManager.Instance.SetCameraMode(currentCCTVIndex);
    }

    public void SwitchMode(PlayerState playerState)
    {
        // switches player state
        this.playerState = playerState;
        
        // switches input action map
        gameInput.SwitchInputMode();

        // switch camera
        if (playerState == PlayerState.CCTV)
        {
            CameraManager.Instance.SetCameraMode(currentCCTVIndex);
        }
        else
        {
            CameraManager.Instance.SetCameraMode();
        }

        // switch UI
        playerCanvas.enabled = false;
        droneCanvas.enabled = false;
        keypadCanvas.enabled = false;
        switch (playerState)
        {
            case PlayerState.PLAYER:
                playerCanvas.enabled = true;
                break;

            case PlayerState.CCTV:
                break;

            case PlayerState.DRONE:
                droneCanvas.enabled = true;
                break;

            case PlayerState.KEYPAD:
                keypadCanvas.enabled = true;
                break;

            default:
                break;
        }
    }
}
