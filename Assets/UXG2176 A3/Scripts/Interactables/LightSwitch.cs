using UnityEngine;

public class LightSwitch : Interactable
{
    [SerializeField] Light spotlight;

    public override void Activate()
    {
        GetComponentInParent<LightSwitchManager>().DeactivateAllSwitches();
        spotlight.enabled = true;
        isActivated = true;
    }

    public override void Deactivate()
    {
        spotlight.enabled = false;
        isActivated = false;
    }
}
