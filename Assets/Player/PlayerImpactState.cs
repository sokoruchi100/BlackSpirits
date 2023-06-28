using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpactState : PlayerBaseState {
    private readonly int IMPACT_HASH = Animator.StringToHash("Impact");

    private const float CROSS_FADE_DURATION = 0.1f;
    private float duration = 1f;
    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter() {
        stateMachine.Animator.CrossFadeInFixedTime(IMPACT_HASH, CROSS_FADE_DURATION);
    }

    public override void Tick(float deltaTime) {
        Move(deltaTime);

        duration -= deltaTime;

        if (duration <= 0f) {
            ReturnToLocomotion();
        }
    }

    public override void Exit() {
    }   
}
