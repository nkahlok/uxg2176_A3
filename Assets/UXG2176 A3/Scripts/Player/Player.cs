using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    GameInput gameInput;

    public enum PlayerState
    {
        PLAYER_1ST,
        PLAYER_3RD,
        CCTV,
        DRONE,
    };

    public PlayerState playerState = PlayerState.PLAYER_3RD;

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

        SwitchMode(PlayerState.PLAYER_3RD);

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
            SwitchMode(PlayerState.PLAYER_3RD);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SwitchMode(PlayerState.CCTV);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SwitchMode(PlayerState.DRONE);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            //SwitchMode(PlayerState.PLAYER_1ST);
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
    }
}
