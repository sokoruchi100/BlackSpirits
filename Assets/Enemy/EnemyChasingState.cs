using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChasingState : EnemyBaseState {
    private readonly int SPEED_HASH = Animator.StringToHash("Speed");
    private readonly int LOCOMOTION_HASH = Animator.StringToHash("Locomotion");

    private const float ANIMATOR_DAMP_TIME = 0.1f;
    private const float CROSS_FADE_DURATION = 0.1f;

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter() {
        stateMachine.NavMeshAgent.enabled = true;
        stateMachine.Animator.CrossFadeInFixedTime(LOCOMOTION_HASH, CROSS_FADE_DURATION);
    }

    public override void Tick(float deltaTime) {
        MoveToPlayer(deltaTime);
        FacePlayer();

        stateMachine.Animator.SetFloat(SPEED_HASH, 1, ANIMATOR_DAMP_TIME, deltaTime);

        if (IsInAttackRange()) {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
        }

        if (!IsInChaseRange()) {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
    }

    public override void Exit() {
        stateMachine.NavMeshAgent.enabled = false;
    }

    private void MoveToPlayer(float deltaTime) {
        if (stateMachine.NavMeshAgent.isOnNavMesh) {
            stateMachine.NavMeshAgent.destination = stateMachine.PlayerHealth.transform.position;
            Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }
        
        stateMachine.NavMeshAgent.velocity = stateMachine.Controller.velocity;
    }
}
