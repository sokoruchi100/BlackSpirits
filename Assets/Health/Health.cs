using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

    private void Start() {
        currentHealth = maxHealth;
    }

    public void DealDamage(int damageAmount) {
        if (currentHealth == 0) return;

        currentHealth  = Mathf.Max(currentHealth - damageAmount, 0);
        Debug.Log("OOF! "+currentHealth+" HP Remaining");
    }
}
