using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState {
    private readonly int MELEE_IMPACT_HASH = Animator.StringToHash("MeleeImpact");

    private const float CROSS_FADE_DURATION = 0.1f;

    private float duration = 1f;
    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter() {
        stateMachine.Animator.CrossFadeInFixedTime(MELEE_IMPACT_HASH, CROSS_FADE_DURATION);
    }

    public override void Tick(float deltaTime) {
        Move(deltaTime);

        duration -= deltaTime;

        if (duration <= 0f) {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
    }

    public override void Exit() {
        
    }
}
