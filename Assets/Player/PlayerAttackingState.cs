using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState {
    private Attack currentAttack;
    private float previousFrameTime;
    private bool alreadyAppliedForce;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine) {
        currentAttack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter() {
        stateMachine.WeaponDamage.SetDamage(currentAttack.Damage);
        stateMachine.Animator.CrossFadeInFixedTime(currentAttack.AnimationName, currentAttack.TransitionDuration);
    }

    public override void Tick(float deltaTime) {
        FaceTarget();
        Move(deltaTime);

        float normalizedTime = GetNormalizedTime();
        if (normalizedTime >= previousFrameTime && normalizedTime < 1f) {
            if (normalizedTime >= currentAttack.ForceTime) {
                TryApplyForce();
            }

            if (stateMachine.InputReader.IsAttacking) {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (stateMachine.Targeter.CurrentTarget != null) {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            } else {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }
        previousFrameTime = normalizedTime;
    }

    public override void Exit() {

    }

    private float GetNormalizedTime() {
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack")) {
            return nextInfo.normalizedTime;
        } else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack")) {
            return currentInfo.normalizedTime;
        } else {
            return 0f;
        }
    }

    private void TryComboAttack(float normalizedTime) {
        if (currentAttack.ComboStateIndex == -1) return;

        if (normalizedTime < currentAttack.ComboAttackTime) return;

        stateMachine.SwitchState(new PlayerAttackingState(
            stateMachine,
            currentAttack.ComboStateIndex
            ));
    }

    private void TryApplyForce() {
        if (alreadyAppliedForce) return;
        stateMachine.ForceReceiver.AddForce(currentAttack.Force * stateMachine.transform.forward);
        alreadyAppliedForce = true;
    }
}
