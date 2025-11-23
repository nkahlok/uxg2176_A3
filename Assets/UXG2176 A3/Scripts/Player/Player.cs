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

    [SerializeField] Canvas pauseCanvas;

    [SerializeField] Canvas playerCanvas;
    [SerializeField] Canvas cctvCanvas;
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
        gameInput.OnPauseAction += GameInput_OnPauseAction;

        playerState = PlayerState.TRANSITION;
        spawnDelayTimer = spawnDelayDuration;
        hasSpawned = false;

        // disable all UI except player
        if (pauseCanvas != null) pauseCanvas.enabled = false;
        if (cctvCanvas != null) cctvCanvas.enabled = false;
        if (droneCanvas != null) droneCanvas.enabled = false;
        if (keypadCanvas != null) keypadCanvas.enabled = false;
    }

    private void Update()
    {
        // spawn delay
        if (!hasSpawned)
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
        //else
        //{
        //    if (Input.GetKeyUp(KeyCode.Alpha1))
        //    {
        //        SwitchMode(PlayerState.PLAYER);
        //    }
        //    else if (Input.GetKeyUp(KeyCode.Alpha2))
        //    {
        //        if (playerState == PlayerState.CCTV)
        //        {
        //            CycleCCTVCamera();
        //        }
        //        else
        //        {
        //            currentCCTVIndex = 0;
        //            SwitchMode(PlayerState.CCTV);
        //        }
        //    }
        //    else if (Input.GetKeyUp(KeyCode.Alpha3))
        //    {
        //        SwitchMode(PlayerState.DRONE);
        //    }
        //}
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
        if (playerCanvas != null) playerCanvas.enabled = false;
        if (cctvCanvas != null) cctvCanvas.enabled = false;
        if (droneCanvas != null) droneCanvas.enabled = false;
        if (keypadCanvas != null) keypadCanvas.enabled = false;

        switch (playerState)
        {
            case PlayerState.PLAYER:
                if (playerCanvas != null) playerCanvas.enabled = true;
                break;

            case PlayerState.CCTV:
                if (cctvCanvas != null) cctvCanvas.enabled = true;
                break;

            case PlayerState.DRONE:
                if (droneCanvas != null) droneCanvas.enabled = true;
                break;

            case PlayerState.KEYPAD:
                if (keypadCanvas != null) keypadCanvas.enabled = true;
                break;

            default:
                break;
        }
    }

    private void GameInput_OnPauseAction(object sender, System.EventArgs e)
    {
        Time.timeScale = 0f;
        pauseCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        if (playerCanvas != null) playerCanvas.enabled = false;
        if (cctvCanvas != null) cctvCanvas.enabled = false;
        if (droneCanvas != null) droneCanvas.enabled = false;
        if (keypadCanvas != null) keypadCanvas.enabled = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        switch (playerState)
        {
            case PlayerState.PLAYER:
                if (playerCanvas != null) playerCanvas.enabled = true;
                break;

            case PlayerState.CCTV:
                if (cctvCanvas != null) cctvCanvas.enabled = true;
                break;

            case PlayerState.DRONE:
                if (droneCanvas != null) droneCanvas.enabled = true;
                break;

            case PlayerState.KEYPAD:
                if (keypadCanvas != null) keypadCanvas.enabled = true;
                break;

            default:
                break;
        }
    }
}
