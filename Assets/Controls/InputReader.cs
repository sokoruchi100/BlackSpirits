using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions {
    public Vector2 MovementValue { get; private set; }
    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action DropEvent;
    public event Action PullupEvent;

    public bool IsAttacking { get; private set; }
    public bool IsBlocking { get; private set; }

    private Controls controls;

    private void Start() {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    private void OnDestroy() {
        controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (!context.performed) return;
        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context) {
        if (!context.performed) return;
        DodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context) {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context) {
        //Controlled by FreeLookCamera
    }

    public void OnTarget(InputAction.CallbackContext context) {
        if (!context.performed) return;
        TargetEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context) {
        if (context.performed) {
            IsAttacking = true;
        }
        
        if (context.canceled) {
            IsAttacking = false;
        }
    }

    public void OnBlock(InputAction.CallbackContext context) {
        if (context.performed) {
            IsBlocking = true;
        }

        if (context.canceled) {
            IsBlocking = false;
        }
    }

    public void OnDrop(InputAction.CallbackContext context) {
        if (!context.performed) return;
        DropEvent?.Invoke();
    }

    public void OnPullup(InputAction.CallbackContext context) {
        if (!context.performed)
            return;
        PullupEvent?.Invoke();
    }
}
