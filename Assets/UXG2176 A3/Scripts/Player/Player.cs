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

        TRANSITION = 100,
    };

    [HideInInspector] public PlayerState playerState = PlayerState.TRANSITION;

    [SerializeField] float spawnDelayDuration = 2f;
    float spawnDelayTimer = 0f;
    bool hasSpawned = false;

    [SerializeField] Canvas playerCanvas;
    [SerializeField] Canvas droneCanvas;

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
                //SwitchMode(PlayerState.CCTV);
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

    public void SwitchMode(PlayerState playerState)
    {
        // switches player state
        this.playerState = playerState;
        
        // switches input action map
        gameInput.SwitchInputMode();

        // switch camera
        CameraManager.Instance.SetCameraMode();

        // switch UI
        playerCanvas.enabled = false;
        droneCanvas.enabled = false;
        switch (playerState)
        {
            case PlayerState.PLAYER:
                playerCanvas.enabled = true;
                break;

            case PlayerState.DRONE:
                droneCanvas.enabled = true;
                break;

            default:
                break;
        }
    }
}
