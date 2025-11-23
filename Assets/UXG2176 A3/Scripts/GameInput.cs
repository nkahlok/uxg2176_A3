using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    InputSystem_Actions inputActions;

    public event EventHandler OnPlayerInteractAction;
    public event EventHandler OnDroneShootAction;
    public event EventHandler OnDroneReloadAction;
    public event EventHandler OnDroneReturnAction;

    [SerializeField] float horizMouseSens = 1f;
    [SerializeField] float vertMouseSens = 1f;

    private void OnEnable()
    {
        inputActions = new InputSystem_Actions();

        // enable all input action maps and their respective actions
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += PlayerInteract_performed;

        inputActions.Drone.Enable();
        inputActions.Drone.Attack.performed += DroneAttack_performed;
        inputActions.Drone.Reload.performed += DroneReload_performed;
        inputActions.Drone.Return.performed += DroneReturn_performed;

        // disable all maps
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputActions.Disable();
    }

    private void OnDisable()
    {
        // disable all maps
        inputActions.Disable();
    }

    public void SwitchInputMode()
    {
        // disable all input action maps
        inputActions.Disable();

        // enable the appropriate input action map
        switch (Player.Instance.playerState)
        {
            case Player.PlayerState.PLAYER:
                inputActions.Player.Enable();
                break;

            case Player.PlayerState.CCTV:
                break;

            case Player.PlayerState.DRONE:
                inputActions.Drone.Enable();
                break;

            case Player.PlayerState.KEYPAD:
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

            case Player.PlayerState.CCTV:
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

            case Player.PlayerState.CCTV:
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

    private void PlayerInteract_performed(InputAction.CallbackContext obj)
    {
        OnPlayerInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void DroneAttack_performed(InputAction.CallbackContext obj)
    {
        OnDroneShootAction?.Invoke(this, EventArgs.Empty);
    }

    private void DroneReload_performed(InputAction.CallbackContext obj)
    {
        OnDroneReloadAction?.Invoke(this, EventArgs.Empty);
    }

    private void DroneReturn_performed(InputAction.CallbackContext obj)
    {
        OnDroneReturnAction?.Invoke(this, EventArgs.Empty);
    }
}
