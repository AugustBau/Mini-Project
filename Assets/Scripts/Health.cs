using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public ParticleSystem deathEffect;
  

    public event Action OnDeath;

    int current;

    public int CurrentHealth => current;   // <-- add this

    void Awake()
    {
        current = maxHealth;
       

    }

    public void TakeDamage(int amount)
    {
        current -= amount;
        if (current <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}

