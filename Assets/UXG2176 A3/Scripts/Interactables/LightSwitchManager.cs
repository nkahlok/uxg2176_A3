using UnityEngine;

public class LightSwitchManager : MonoBehaviour
{
    [SerializeField] LightSwitch[] lightSwitches;

    private void Start()
    {
        DeactivateAllSwitches();
    }

    public void DeactivateAllSwitches()
    {
        foreach (LightSwitch lightSwitch in lightSwitches)
        {
            lightSwitch.Deactivate();
        }
    }
}
