using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    
    [SerializeField] Camera playerCamera;
    [SerializeField] Camera droneCamera;

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

    public void SetCameraMode(Player.PlayerState playerState)
    {
        playerCamera.enabled = (playerState == Player.PlayerState.PLAYER);
        droneCamera.enabled = (playerState == Player.PlayerState.DRONE);

        switch (playerState)
        {
            case Player.PlayerState.PLAYER:
                playerCamera.GetComponent<ThirdPersonCamera>().InitCameraPosition();
                break;

            case Player.PlayerState.DRONE:
                droneCamera.GetComponent<ThirdPersonCamera>().InitCameraPosition();
                break;

            default:
                break;
        }
    }
}
