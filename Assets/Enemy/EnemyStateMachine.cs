using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    private const string PLAYER_TAG = "Player";
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float PlayerChasingRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public float AttackKnockback { get; private set; }
    public Health PlayerHealth { get; private set; }

    private void Start() {
        PlayerHealth = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<Health>();
        NavMeshAgent.updatePosition = false;
        NavMeshAgent.updateRotation = false;
        SwitchState(new EnemyIdleState(this));
    }

    private void OnEnable() {
        Health.OnTakeDamage += Impact;
        Health.OnDie += Die;
    }

    private void OnDisable() {
        Health.OnTakeDamage -= Impact;
        Health.OnDie -= Die;
    }

    private void Impact() {
        SwitchState(new EnemyImpactState(this));
    }

    private void Die() {
        SwitchState(new EnemyDeadState(this));
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
    }
}
