using UnityEngine;

public class LightSwitchManager : MonoBehaviour
{
    [SerializeField] LightSwitch[] lightSwitches;
    int numSwitchesActivatedBefore = 0;

    [SerializeField] ObjectiveText objectiveText;

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

    public void UpdateNumSwitchesActivatedBefore()
    {
        numSwitchesActivatedBefore++;

        switch (numSwitchesActivatedBefore)
        {
            case 0:
                objectiveText.UpdateObjText(ObjectiveText.ObjText.SHADOW_LIGHTSWITCHES_0);
                break;

            case 1:
                objectiveText.UpdateObjText(ObjectiveText.ObjText.SHADOW_LIGHTSWITCHES_1);
                break;

            case 2:
                objectiveText.UpdateObjText(ObjectiveText.ObjText.SHADOW_LIGHTSWITCHES_2);
                break;

            case 3:
                objectiveText.UpdateObjText(ObjectiveText.ObjText.SHADOW_LIGHTSWITCHES_3);
                break;

            case 4:
                objectiveText.UpdateObjText(ObjectiveText.ObjText.SHADOW_KEYPAD);
                break;
        }
    }
}
