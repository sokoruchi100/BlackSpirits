using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullupState : PlayerBaseState {
    private readonly int PULLUP_HASH = Animator.StringToHash("Pullup");

    private const float CROSS_FADE_DURATION = 0.1f;
    private readonly Vector3 PULLUP_MOVE_VECTOR = new Vector3(0, 2.325f, 0.65f);
    public PlayerPullupState(PlayerStateMachine stateMachine) : base(stateMachine) {
    }

    public override void Enter() {
        stateMachine.Animator.CrossFadeInFixedTime(PULLUP_HASH, CROSS_FADE_DURATION);
    }

    public override void Tick(float deltaTime) {
        if (GetNormalizedTime(stateMachine.Animator, "Climbing") < 1) return;
        stateMachine.Controller.enabled = false;
        stateMachine.transform.Translate(PULLUP_MOVE_VECTOR, Space.Self);
        stateMachine.Controller.enabled = true;
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine, false));
    }

    public override void Exit() {
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
    }
}
