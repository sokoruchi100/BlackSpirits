using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnDie;

    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

    private void Start() {
        currentHealth = maxHealth;
    }

    public void DealDamage(int damageAmount) {
        if (currentHealth == 0) return;

        OnTakeDamage?.Invoke();
        currentHealth  = Mathf.Max(currentHealth - damageAmount, 0);

        if (currentHealth == 0) {
            OnDie?.Invoke();
        }

        Debug.Log("OOF! "+currentHealth+" HP Remaining");
    }
}
