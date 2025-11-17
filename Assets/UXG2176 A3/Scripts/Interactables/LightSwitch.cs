using UnityEngine;

public class LightSwitch : Interactable
{
    [SerializeField] Light spotlight;

    public override void Activate()
    {
        GetComponentInParent<LightSwitchManager>().DeactivateAllSwitches();
        spotlight.enabled = true;
        base.Activate();
    }

    public override void Deactivate()
    {
        spotlight.enabled = false;
        base.Deactivate();
    }
}
