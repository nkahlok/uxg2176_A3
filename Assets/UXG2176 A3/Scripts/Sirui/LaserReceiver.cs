using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserReceiver : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private bool requiresContinuousHit = false;
    [SerializeField] private float requiredHitDuration = 2f; // How long laser needs to hit
    [SerializeField] private bool loadNextLevel = true;
    
    [Header("Animation Settings")]
    [SerializeField] private Vector3 openPosition = Vector3.up * 3f;
    [SerializeField] private float openSpeed = 2f;
    
    [Header("Visual Feedback")]
    [SerializeField] private Material inactiveMaterial;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private MeshRenderer doorRenderer;

    private bool isBeingHit = false;
    private float hitTimer = 0f;
    private bool isDoorOpen = false;
    private Vector3 closedPosition;
    private Vector3 targetPosition;
    private bool hasPlayedHitSound = false;

    private void Start()
    {
        closedPosition = transform.position;
        targetPosition = closedPosition;

        if (doorRenderer != null && inactiveMaterial == null)
        {
            inactiveMaterial = doorRenderer.material;
        }
    }

    private void Update()
    {
        if (requiresContinuousHit && isBeingHit)
        {
            hitTimer += Time.deltaTime;
            
            if (hitTimer >= requiredHitDuration && !isDoorOpen)
            {
                OpenDoor();
            }
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * openSpeed);
    }

    public void OnLaserHit()
    {
        isBeingHit = true;

        if (doorRenderer != null && activeMaterial != null)
        {
            doorRenderer.material = activeMaterial;
        }

        if (!hasPlayedHitSound && LaserPuzzleAudioManager.Instance != null)
        {
            LaserPuzzleAudioManager.Instance.PlayReceiverHit();
            hasPlayedHitSound = true;
            
            // If continuous hit required, play charging sound
            if (requiresContinuousHit)
            {
                LaserPuzzleAudioManager.Instance.PlayReceiverCharging();
            }
        }
        if (!requiresContinuousHit && !isDoorOpen)
        {
            OpenDoor();
        }
    }

    public void OnLaserExit()
    {
        isBeingHit = false;
        hitTimer = 0f;
        hasPlayedHitSound = false;
        if (doorRenderer != null && inactiveMaterial != null)
        {
            doorRenderer.material = inactiveMaterial;
        }
        if (LaserPuzzleAudioManager.Instance != null)
        {
            LaserPuzzleAudioManager.Instance.StopReceiverCharging();
        }
    }

    private void OpenDoor()
    {
        isDoorOpen = true;
        targetPosition = closedPosition + openPosition;

        if (LaserPuzzleAudioManager.Instance != null)
        {
            LaserPuzzleAudioManager.Instance.StopReceiverCharging();
            LaserPuzzleAudioManager.Instance.PlayDoorOpen();
            LaserPuzzleAudioManager.Instance.PlaySuccess();
        }

        if (loadNextLevel)
        {
            Invoke(nameof(LoadNextScene), 2f);
        }
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
