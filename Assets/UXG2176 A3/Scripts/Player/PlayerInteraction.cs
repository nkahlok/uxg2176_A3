using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    GameInput gameInput;

    bool isPlayerWithinInteractionRange = false;
    bool canInteract = false;
    [SerializeField] float maxRange = 3f;
    [SerializeField] LayerMask hitLayers;
    Transform hitTransform;

    [SerializeField] TMP_Text interactText;

    private void Start()
    {
        gameInput = GetComponent<GameInput>();
        gameInput.OnPlayerInteractAction += GameInput_OnPlayerInteractAction;

        interactText.enabled = false;
    }

    public void SetIsPlayerWithinInteractionRange(bool isPlayerWithinInteractionRange)
    {
        this.isPlayerWithinInteractionRange = isPlayerWithinInteractionRange;
    }

    private void GameInput_OnPlayerInteractAction(object sender, System.EventArgs e)
    {
        if (canInteract)
        {
            hitTransform.GetComponent<Interactable>().Activate();
        }
    }

    private void Update()
    {
        if (isPlayerWithinInteractionRange)
        {
            if (CheckIfLookingAtInteractable())
            {
                canInteract = true;
                interactText.enabled = true;
            }
            else
            {
                canInteract = false;
                interactText.enabled = false;
            }
        }
        else
        {
            canInteract = false;
            interactText.enabled = false;
        }
    }

    private bool CheckIfLookingAtInteractable()
    {
        // shoots a ray from centre of screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, maxRange, hitLayers))
        {
            hitTransform = hit.transform.parent;
            return true;
        }
        else
        {
            hitTransform = null;
            return false;
        }
    }
}
