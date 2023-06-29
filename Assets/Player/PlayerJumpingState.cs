using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState {
    private readonly int JUMP_HASH = Animator.StringToHash("Jump");

    private const float CROSS_FADE_DURATION = 0.1f;

    private Vector3 momentum;
    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine) {
    }

    public override void Enter() {
        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0f;
        stateMachine.Animator.CrossFadeInFixedTime(JUMP_HASH, CROSS_FADE_DURATION);
        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetection;
    }

    public override void Tick(float deltaTime) {
        Move(momentum, deltaTime);

        if (stateMachine.Controller.velocity.y <= 0f) {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
        }

        FaceTarget();
    }

    public override void Exit() {
        stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetection;
    }

    private void HandleLedgeDetection(Vector3 ledgeForward, Vector3 closestPoint) {
        stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward, closestPoint));
    }
}
