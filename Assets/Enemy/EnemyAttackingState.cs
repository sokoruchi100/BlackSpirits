using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState {
    private readonly int MELEE_ATTACK_HASH = Animator.StringToHash("MeleeAttack");

    private const float TRANSITION_DURATION = 0.1f;
    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter() {
        stateMachine.Weapon.SetAttack(stateMachine.AttackDamage, stateMachine.AttackKnockback);

        stateMachine.Animator.CrossFadeInFixedTime(MELEE_ATTACK_HASH, TRANSITION_DURATION);
    }

    public override void Tick(float deltaTime) {
        if (GetNormalizedTime(stateMachine.Animator) >= 1) {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
    }

    public override void Exit() {
        
    }
}
