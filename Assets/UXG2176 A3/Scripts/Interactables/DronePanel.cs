using UnityEngine;

public class DronePanel : Interactable
{
    [SerializeField] ObjectiveText objectiveText;
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        objectiveText.UpdateObjText(ObjectiveText.ObjText.SHADOW_DRONEPANEL);
    }

    public override void Activate()
    {
        objectiveText.UpdateObjText(ObjectiveText.ObjText.SHADOW_LIGHTSWITCHES_0);
        audioSource.Play();
        Player.Instance.SwitchMode(Player.PlayerState.DRONE);
    }
}
