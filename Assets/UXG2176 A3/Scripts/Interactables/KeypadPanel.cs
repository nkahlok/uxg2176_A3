public class KeypadPanel : Interactable
{
    Keypad keypad;

    private void Start()
    {
        keypad = transform.GetChild(0).GetComponent<Keypad>();
    }

    public override void Activate()
    {
        if (!keypad.isUnlocked)
        {
            Player.Instance.SwitchMode(Player.PlayerState.KEYPAD);
        }
    }
}
