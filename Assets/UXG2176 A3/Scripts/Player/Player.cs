using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    GameInput gameInput;

    [Space(10f)]
    [Header("DEBUG")]
    [SerializeField] bool doLockCursor = true;

    public enum PlayerState
    {
        PLAYER,
        CAMERA,
        DRONE,
    };

    public PlayerState playerState = PlayerState.PLAYER;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        gameInput = GetComponent<GameInput>();

        // DEBUG LOCK CURSOR
        if (doLockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SwitchMode(PlayerState playerState)
    {
        // switches player state
        this.playerState = playerState;
        
        // switches input action map based on the player state
        gameInput.SwitchInputMode(playerState);
    }
}
