using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    InputSystem_Actions inputActions;

    static float horizMouseSens = 8f;
    static float vertMouseSens = 7f;

    private void OnEnable()
    {
        inputActions = new InputSystem_Actions();

        // enable all input action maps and their respective actions
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += PlayerInteract_performed;

        inputActions.Drone.Enable();
        inputActions.Drone.Attack.performed += DroneAttack_performed;
        inputActions.Drone.Attack.canceled += DroneAttack_canceled;

        // disable all maps except for player
        //inputActions.Drone.Disable();
    }

    private void OnDisable()
    {
        // disable all maps
        inputActions.Player.Disable();
        inputActions.Drone.Disable();
    }

    public void SwitchInputMode(Player.PlayerState playerState)
    {
        // disable all input action maps
        inputActions.Player.Disable();
        inputActions.Drone.Disable();

        // enable the appropriate input action map
        switch (playerState)
        {
            case Player.PlayerState.PLAYER:
                inputActions.Player.Enable();
                break;

            case Player.PlayerState.CAMERA:
                break;

            case Player.PlayerState.DRONE:
                inputActions.Drone.Enable();
                break;

            default:
                break;
        }
    }

    public Vector3 GetMovementVector()
    {
        // returns the relevant input action map movement vector
        switch (Player.Instance.playerState)
        {
            case Player.PlayerState.PLAYER:
                return inputActions.Player.Move.ReadValue<Vector2>();

            case Player.PlayerState.CAMERA:
                return Vector3.zero;

            case Player.PlayerState.DRONE:
                return inputActions.Drone.Move.ReadValue<Vector3>();

            default:
                return Vector3.zero;
        }
    }

    public Vector2 GetMouseVector()
    {
        // returns the relevant input action map mouse vector
        switch (Player.Instance.playerState)
        {
            case Player.PlayerState.PLAYER:
                return inputActions.Player.Look.ReadValue<Vector2>();

            case Player.PlayerState.CAMERA:
                return Vector3.zero;

            case Player.PlayerState.DRONE:
                return inputActions.Drone.Look.ReadValue<Vector2>();

            default:
                return Vector3.zero;
        }
    }

    public Vector2 GetMouseSens()
    {
        return new Vector2 (horizMouseSens, vertMouseSens);
    }

    private void PlayerInteract_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

    }

    private void DroneAttack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

    }

    private void DroneAttack_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

    }
}
