using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState {
    private readonly int DODGE_BLEND_TREE_HASH = Animator.StringToHash("DodgeBlendTree");
    private readonly int DODGE_FORWARD_HASH = Animator.StringToHash("DodgeForward");
    private readonly int DODGE_RIGHT_HASH = Animator.StringToHash("DodgeRight");

    private const float CROSS_FADE_DURATION = 0.1f;

    private Vector3 dodgingDirectionInput;
    private float remainingDodgeTime;

    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine) {
        this.dodgingDirectionInput = dodgingDirectionInput;
    }

    public override void Enter() {
        remainingDodgeTime = stateMachine.DodgeDuration;
        stateMachine.Health.SetInvulnerable(true);

        stateMachine.Animator.SetFloat(DODGE_RIGHT_HASH, dodgingDirectionInput.x);
        stateMachine.Animator.SetFloat(DODGE_FORWARD_HASH, dodgingDirectionInput.y);

        stateMachine.Animator.CrossFadeInFixedTime(DODGE_BLEND_TREE_HASH, CROSS_FADE_DURATION);
    }

    public override void Tick(float deltaTime) {
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.DodgeDistance / stateMachine.DodgeDuration;
        movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeDistance / stateMachine.DodgeDuration;

        Move(movement, deltaTime);

        FaceTarget();

        remainingDodgeTime -= deltaTime;

        if (remainingDodgeTime <= 0) {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            return;
        }
    }

    public override void Exit() {
        stateMachine.Health.SetInvulnerable(false);
    }
}
