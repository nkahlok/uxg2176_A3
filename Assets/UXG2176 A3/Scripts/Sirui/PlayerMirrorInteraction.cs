using UnityEngine;

public class PlayerMirrorInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask interactableLayer;
    
    private GameInput gameInput;
    private RotatableMirror currentMirror = null;

    private void Start()
    {
        gameInput = GetComponent<GameInput>();
        gameInput.OnPlayerInteractAction += GameInput_OnPlayerInteractAction;
    }

    private void Update()
    {
        CheckForNearbyMirrors();
    }

    private void CheckForNearbyMirrors()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);
        
        RotatableMirror closestMirror = null;
        float closestDistance = interactionRange;

        foreach (Collider col in hitColliders)
        {
            RotatableMirror mirror = col.GetComponent<RotatableMirror>();
            if (mirror != null)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestMirror = mirror;
                }
            }
        }

        currentMirror = closestMirror;
    }

    private void GameInput_OnPlayerInteractAction(object sender, System.EventArgs e)
    {
        if (currentMirror != null && Player.Instance.playerState == Player.PlayerState.PLAYER)
        {
            currentMirror.Activate();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
