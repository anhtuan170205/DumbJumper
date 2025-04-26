using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;
using System;


[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action ShootEvent;
    public event Action PullEvent;
    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    public Vector2 AimPosition { get; private set; }
    private PlayerControls controls;
    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new PlayerControls();
            controls.Player.SetCallbacks(this);
        }
        controls.Player.Enable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShootEvent?.Invoke();
        }
    }
    public void OnPull(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PullEvent?.Invoke();
        }

    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpEvent?.Invoke();
        }
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        AimPosition = context.ReadValue<Vector2>();
    }

}
