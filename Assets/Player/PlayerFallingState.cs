using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState {
    private readonly int FALL_HASH = Animator.StringToHash("Fall");

    private const float CROSS_FADE_DURATION = 0.1f;

    private Vector3 momentum;
    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine) {
    }

    public override void Enter() {
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0f;
        stateMachine.Animator.CrossFadeInFixedTime(FALL_HASH, CROSS_FADE_DURATION);
        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetection;
    }

    public override void Tick(float deltaTime) {
        Move(momentum, deltaTime);

        if (stateMachine.Controller.isGrounded) {
            ReturnToLocomotion();
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
