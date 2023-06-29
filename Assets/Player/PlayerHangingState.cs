using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState {
    private readonly int HANGING_HASH = Animator.StringToHash("Hanging");

    private const float CROSS_FADE_DURATION = 0.1f;

    private Vector3 ledgeForward;
    private Vector3 closestPoint;
    public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(stateMachine) {
        this.ledgeForward = ledgeForward;
        this.closestPoint = closestPoint;
    }

    public override void Enter() {
        stateMachine.Controller.enabled = false;
        stateMachine.transform.position = closestPoint - (stateMachine.LedgeDetector.transform.position - stateMachine.transform.position);
        stateMachine.Controller.enabled = true;

        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);

        stateMachine.Animator.CrossFadeInFixedTime(HANGING_HASH, CROSS_FADE_DURATION);
        stateMachine.InputReader.DropEvent += OnDrop;
        stateMachine.InputReader.PullupEvent += OnPullup;
    }

    public override void Tick(float deltaTime) {

    }

    public override void Exit() {
        stateMachine.InputReader.DropEvent -= OnDrop;
        stateMachine.InputReader.PullupEvent -= OnPullup;
    }

    private void OnDrop() {
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
        stateMachine.SwitchState(new PlayerFallingState(stateMachine));
    }

    private void OnPullup() {
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
        stateMachine.SwitchState(new PlayerPullupState(stateMachine));
    }
}
