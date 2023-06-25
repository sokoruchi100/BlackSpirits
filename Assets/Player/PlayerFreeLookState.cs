using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState {
    private readonly int FREE_LOOK_SPEED_HASH = Animator.StringToHash("FreeLookSpeed");
    private readonly int FREE_LOOK_BLEND_TREE_HASH = Animator.StringToHash("FreeLookBlendTree");
    
    private const float ANIMATOR_DAMP_TIME = 0.1f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter() {
        stateMachine.InputReader.TargetEvent += ToggleTargeting;
        stateMachine.Animator.Play(FREE_LOOK_BLEND_TREE_HASH);
    }

    public override void Tick(float deltaTime) {
        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.FreeLookMovementSpeed, Time.deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero) {
            stateMachine.Animator.SetFloat(FREE_LOOK_SPEED_HASH, 0, ANIMATOR_DAMP_TIME, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(FREE_LOOK_SPEED_HASH, 1, ANIMATOR_DAMP_TIME, deltaTime);
        
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit() {
        stateMachine.InputReader.TargetEvent -= ToggleTargeting;
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
