using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnDie;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int damageReduction = 2;

    private int currentHealth;
    private bool isInvulnerable;
    private bool isBlocking;

    public bool IsDead => currentHealth == 0;

    private void Start() {
        currentHealth = maxHealth;
    }

    public void DealDamage(int damageAmount) {
        if (currentHealth == 0) return;
        if (isInvulnerable) return;

        
        if (isBlocking) {
            currentHealth = Mathf.Max(currentHealth - (damageAmount/damageReduction), 0);
        } else {
            OnTakeDamage?.Invoke();
            currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        }
        

        if (currentHealth == 0) {
            OnDie?.Invoke();
        }
    }

    public void SetInvulnerable(bool isInvulnerable) {
        this.isInvulnerable = isInvulnerable;
    }

    public void SetBlocking(bool isBlocking) {
        this.isBlocking = isBlocking;
    }
}
