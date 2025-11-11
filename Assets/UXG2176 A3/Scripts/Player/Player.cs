using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    GameInput gameInput;

    public enum PlayerState
    {
        PLAYER,
        CAMERA,
        DRONE,
    };

    public PlayerState playerState = PlayerState.PLAYER;

    [Space(10f)]
    [Header("DEBUG")]
    [SerializeField] bool doLockCursor = true;

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

        playerState = PlayerState.PLAYER;
        CameraManager.Instance.SetCameraMode(playerState);

        // DEBUG LOCK CURSOR
        if (doLockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            SwitchMode(PlayerState.PLAYER);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SwitchMode(PlayerState.CAMERA);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SwitchMode(PlayerState.DRONE);
        }
    }

    public void SwitchMode(PlayerState playerState)
    {
        // switches player state
        this.playerState = playerState;
        
        // switches input action map based on the player state
        gameInput.SwitchInputMode(playerState);

        // switch camera
        CameraManager.Instance.SetCameraMode(playerState);
    }
}
