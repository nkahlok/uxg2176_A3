using UnityEngine;

public class LightSwitch : Interactable
{
    [SerializeField] Light spotlight;
    public bool hasBeenActivated = false;

    public override void Activate()
    {
        GetComponentInParent<LightSwitchManager>().DeactivateAllSwitches();
        spotlight.enabled = true;
        isActivated = true;

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
    }
}
