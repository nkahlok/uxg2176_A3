public class DronePanel : Interactable
{
    public override void Activate()
    {
        Player.Instance.SwitchMode(Player.PlayerState.DRONE);
    }
}
