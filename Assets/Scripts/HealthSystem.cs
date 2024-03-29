using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

    public event EventHandler OnDead;
    public event EventHandler OnDamage;
    
    [SerializeField] private int health = 100;
    private float healthMax;

    private void Awake() {
        healthMax = health;
    }

    public void TakeDamage(int damageAmount) {
        health -= damageAmount;
        OnDamage?.Invoke(this, EventArgs.Empty);
        
        if (health < 0) {
            health = 0;
        }

        if (health == 0) {
            Die();
        }
    }

    private void Die() {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized() {
        return (float)health / healthMax;
    }
}
