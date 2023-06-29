using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState {
    private readonly int FREE_LOOK_SPEED_HASH = Animator.StringToHash("FreeLookSpeed");
    private readonly int FREE_LOOK_BLEND_TREE_HASH = Animator.StringToHash("FreeLookBlendTree");
    
    private const float ANIMATOR_DAMP_TIME = 0.1f;
    private const float CROSS_FADE_DURATION = 0.1f;
    private bool shouldFade;
    public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine) {
        this.shouldFade = shouldFade;
    }

    public override void Enter() {
        stateMachine.InputReader.TargetEvent += ToggleTargeting;
        stateMachine.InputReader.JumpEvent += OnJump;

        stateMachine.Animator.SetFloat(FREE_LOOK_SPEED_HASH, 0);

        if (shouldFade) {
            stateMachine.Animator.CrossFadeInFixedTime(FREE_LOOK_BLEND_TREE_HASH, CROSS_FADE_DURATION);
        } else {
            stateMachine.Animator.Play(FREE_LOOK_BLEND_TREE_HASH);
        }
    }

    public override void Tick(float deltaTime) {
        if (stateMachine.InputReader.IsAttacking) {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        if (stateMachine.InputReader.IsBlocking) {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();

        if (movement != Vector3.zero) {
            FaceMovementDirection(movement, deltaTime);
        }

        Move(movement * stateMachine.FreeLookMovementSpeed, Time.deltaTime);

        stateMachine.Animator.SetFloat(FREE_LOOK_SPEED_HASH, stateMachine.InputReader.MovementValue.magnitude, ANIMATOR_DAMP_TIME, deltaTime);
    }

    public override void Exit() {
        stateMachine.InputReader.TargetEvent -= ToggleTargeting;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnJump() {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    private Vector3 CalculateMovement() {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime) {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), deltaTime * stateMachine.RotationDamping);
    }

    private void ToggleTargeting() {
        if (!stateMachine.Targeter.TrySelectTarget()) return;

        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }
}
