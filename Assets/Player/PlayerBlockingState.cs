using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState {
    private readonly int BLOCK_HASH = Animator.StringToHash("Block");

    private const float CROSS_FADE_DURATION = 0.1f;
    private float timer = CROSS_FADE_DURATION;
    private bool hasFinished = false;

    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter() {
        stateMachine.Animator.CrossFadeInFixedTime(BLOCK_HASH, CROSS_FADE_DURATION);
    }

    public override void Tick(float deltaTime) {
        Move(deltaTime);

        if (!hasFinished) {
            timer -= deltaTime;
            if (timer <= 0) {
                stateMachine.Health.SetBlocking(true);
                hasFinished = true;
            }
        }
        
        if (!stateMachine.InputReader.IsBlocking) {
            if (stateMachine.Targeter.CurrentTarget != null) {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            } else {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }
    }

    public override void Exit() {
        stateMachine.Health.SetBlocking(false);
    }
}
