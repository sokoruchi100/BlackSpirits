using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State {
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine) {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime) {
        stateMachine.Controller.Move((stateMachine.ForceReceiver.Movement + motion) * deltaTime);
    }

    protected void Move(float deltaTime) {
        Move(Vector3.zero, deltaTime);
    }

    protected void FaceTarget() {
        if (stateMachine.Targeter.CurrentTarget == null)
            return;
        Vector3 faceDirection = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        faceDirection.y = 0;

        stateMachine.transform.rotation = Quaternion.LookRotation(faceDirection);
    }
}
