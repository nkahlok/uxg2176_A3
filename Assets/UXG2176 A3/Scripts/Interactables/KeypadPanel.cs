using UnityEngine;

public class KeypadPanel : Interactable
{
    GameInput gameInput;
    Keypad keypad;

    private void Start()
    {
        gameInput = Player.Instance.GetComponent<GameInput>();
        gameInput.OnKeypadReturnAction += GameInput_OnKeypadReturnAction;

        keypad = transform.GetChild(0).GetComponent<Keypad>();
    }

    private void GameInput_OnKeypadReturnAction(object sender, System.EventArgs e)
    {
        keypad.ClearInput();
        Player.Instance.SwitchMode(Player.PlayerState.PLAYER);
    }

    public override void Activate()
    {
        if (!keypad.isUnlocked)
        {
            Player.Instance.SwitchMode(Player.PlayerState.KEYPAD);
        }
    }
}
