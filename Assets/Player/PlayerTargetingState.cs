using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState {
    private readonly int TARGETING_BLEND_TREE_HASH = Animator.StringToHash("TargetingBlendTree");
    private readonly int TARGETING_FORWARD_HASH = Animator.StringToHash("TargetingForward");
    private readonly int TARGETING_RIGHT_HASH = Animator.StringToHash("TargetingRight");

    private const float ANIMATOR_DAMP_TIME = 0.1f;
    private const float CROSS_FADE_DURATION = 0.1f;

    private float dodgeTimer;

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter() {
        dodgeTimer = stateMachine.DodgeCooldown;
        stateMachine.InputReader.TargetEvent += ToggleTargeting;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.Animator.CrossFadeInFixedTime(TARGETING_BLEND_TREE_HASH, CROSS_FADE_DURATION);
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

        if (stateMachine.Targeter.CurrentTarget == null) {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        FaceTarget();
        Vector3 movement = CalculateMovement(deltaTime);
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        dodgeTimer -= Time.deltaTime;
    }

    public override void Exit() {
        stateMachine.InputReader.TargetEvent -= ToggleTargeting;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnJump() {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    private void OnDodge() {
        if (stateMachine.InputReader.MovementValue == Vector2.zero) return;

        if (dodgeTimer > 0) return;

        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
    }

    private void ToggleTargeting() {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private Vector3 CalculateMovement(float deltaTime) {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    private void UpdateAnimator(float deltaTime) {
        stateMachine.Animator.SetFloat(TARGETING_FORWARD_HASH, stateMachine.InputReader.MovementValue.y, ANIMATOR_DAMP_TIME, deltaTime);

        stateMachine.Animator.SetFloat(TARGETING_RIGHT_HASH, stateMachine.InputReader.MovementValue.x, ANIMATOR_DAMP_TIME, deltaTime);
    }
}
