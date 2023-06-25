using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState {
    private readonly int TARGETING_BLEND_TREE_HASH = Animator.StringToHash("TargetingBlendTree");

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter() {
        stateMachine.InputReader.TargetEvent += ToggleTargeting;
        stateMachine.Animator.Play(TARGETING_BLEND_TREE_HASH);
    }

    public override void Tick(float deltaTime) {
        if (stateMachine.Targeter.CurrentTarget == null) {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }
    }

    public override void Exit() {
        stateMachine.InputReader.TargetEvent -= ToggleTargeting;
    }
    
    private void ToggleTargeting() {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
}
