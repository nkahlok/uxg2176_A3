using TMPro;
using UnityEngine;

public class ObjectiveText : MonoBehaviour
{
    [SerializeField] TMP_Text objectiveText;

    public enum ObjText
    {
        SHADOW_DRONEPANEL = 0,
        SHADOW_LIGHTSWITCHES_0,
        SHADOW_LIGHTSWITCHES_1,
        SHADOW_LIGHTSWITCHES_2,
        SHADOW_LIGHTSWITCHES_3,
        SHADOW_KEYPAD,

        FIXEDCAM_CAMPANEL = 10,
        FIXEDCAM_KEYPAD,
        FIXEDCAM_NEXTLEVEL,

        NEXTLEVEL = 100,
    };

    public void UpdateObjText(ObjText objText)
    {
        switch (objText)
        {
            case ObjText.SHADOW_DRONEPANEL:
                objectiveText.text = "Interact with the Drone Control Panel to access the drone.";
                break;

            case ObjText.SHADOW_LIGHTSWITCHES_0:
                objectiveText.text = "0/4 Light Switches toggled\nLeft click to shoot bullets from the drone to toggle a Light Switch.";
                break;

            case ObjText .SHADOW_LIGHTSWITCHES_1:
                objectiveText.text = "1/4 Light Switches toggled\nLeft click to shoot bullets from the drone to toggle a Light Switch.";
                break;

            case ObjText.SHADOW_LIGHTSWITCHES_2:
                objectiveText.text = "2/4 Light Switches toggled\nLeft click to shoot bullets from the drone to toggle a Light Switch.";
                break;

            case ObjText.SHADOW_LIGHTSWITCHES_3:
                objectiveText.text = "3/4 Light Switches toggled\nLeft click to shoot bullets from the drone to toggle a Light Switch.";
                break;

            case ObjText.SHADOW_KEYPAD:
                objectiveText.text = "4/4 Light Switches toggled\nPress Z to exit the drone and return back to the player.\nEnter a 4-digit code into the keypad to unlock the door.";
                break;

            case ObjText.NEXTLEVEL:
                objectiveText.text = "Head for the door!";
                break;

            case ObjText.FIXEDCAM_CAMPANEL:
                objectiveText.text = "Interact with the CCTV Control Panel to access the CCTVs.";
                break;

            case ObjText.FIXEDCAM_KEYPAD:
                objectiveText.text = "Press X and C to cycle between cameras.\nPress Z to exit the CCTVs and return back to the player.\nEnter a 4-digit code into the keypad to unlock the door.";
                break;

            default:
                objectiveText.text = string.Empty;
                break;
        }
    }
}
