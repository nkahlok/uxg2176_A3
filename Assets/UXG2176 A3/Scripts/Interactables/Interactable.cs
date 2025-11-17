using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isPlayerWithinRange { get; private set; } = false;
    bool isActivated = false;
    
    public virtual void Activate()
    {

    }

    public virtual void Deactivate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerWithinRange = true;
        }
        else if (other.tag == "Projectile")
        {
            if (!isActivated)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }

            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerWithinRange = false;
        }
    }
}
