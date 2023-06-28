using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState {
    private readonly int SPEED_HASH = Animator.StringToHash("Speed");
    private readonly int LOCOMOTION_HASH = Animator.StringToHash("Locomotion");

    private const float ANIMATOR_DAMP_TIME = 0.1f;
    private const float CROSS_FADE_DURATION = 0.1f;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter() {
        stateMachine.Animator.CrossFadeInFixedTime(LOCOMOTION_HASH, CROSS_FADE_DURATION);
    }

    public override void Tick(float deltaTime) {
        Move(deltaTime);
        if (IsInChaseRange()) {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }

        stateMachine.Animator.SetFloat(SPEED_HASH, 0, ANIMATOR_DAMP_TIME, deltaTime);
    }

    public override void Exit() {
        
    }
}
