using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State {
    protected EnemyStateMachine stateMachine;
    public EnemyBaseState(EnemyStateMachine stateMachine) {
        this.stateMachine = stateMachine;
    }

    protected bool IsInChaseRange() {
        Vector3 toPlayer = stateMachine.Player.transform.position - stateMachine.transform.position;
        
        return (toPlayer.sqrMagnitude <= (stateMachine.PlayerChasingRange*stateMachine.PlayerChasingRange));
    }

    protected bool IsInAttackRange() {
        Vector3 toPlayer = stateMachine.Player.transform.position - stateMachine.transform.position;

        return (toPlayer.sqrMagnitude <= (stateMachine.AttackRange * stateMachine.AttackRange));
    }

    protected void FacePlayer() {
        if (stateMachine.Player == null) return;

        Vector3 faceDirection = stateMachine.Player.transform.position - stateMachine.transform.position;
        faceDirection.y = 0;

        stateMachine.transform.rotation = Quaternion.LookRotation(faceDirection);
    }

    protected void Move(float deltaTime) {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 movement, float deltaTime) {
        stateMachine.Controller.Move((stateMachine.ForceReceiver.Movement + movement) * deltaTime);
    }
}
