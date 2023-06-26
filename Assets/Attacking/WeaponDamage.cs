using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider playerCollider;

    private List<Collider> alreadyCollidedWith = new List<Collider>();
    private int weaponDamage;

    private void OnEnable() {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other) {
        if (other == playerCollider) return;

        if (alreadyCollidedWith.Contains(other)) return;

        alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<Health>(out Health health)) {
            health.DealDamage(weaponDamage);
        }
    }

    public void SetDamage(int damageAmount) {
        weaponDamage = damageAmount;
    }
}
