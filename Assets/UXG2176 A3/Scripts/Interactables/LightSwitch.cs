using UnityEngine;

public class LightSwitch : Interactable
{
    [SerializeField] Light spotlight;
    public bool hasBeenActivated = false;

    [SerializeField] AudioClip switchFlick;
    [SerializeField] AudioSource audioSource;

    public override void Activate()
    {
        GetComponentInParent<LightSwitchManager>().DeactivateAllSwitches();
        spotlight.enabled = true;
        isActivated = true;

        audioSource.PlayOneShot(switchFlick);

        if (!hasBeenActivated)
        {
            hasBeenActivated = true;
            GetComponentInParent<LightSwitchManager>().UpdateNumSwitchesActivatedBefore();
        }
    }

    public override void Deactivate()
    {
        spotlight.enabled = false;
        isActivated = false;

        if (hasBeenActivated)
        {
            audioSource.PlayOneShot(switchFlick);
        }
    }
}
