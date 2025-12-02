using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public ParticleSystem deathEffect;
    public bool destroyOnDeath = true;   // NEW – can be turned off for player
    public bool isPlayer = false;      // tick this on the player


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
        if (isPlayer)
        {
            // Tell GameManager the player died
            GameManager.Instance.OnPlayerDied();
        }
        else
        {
            // Normal enemy death
            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            }

            GameManager.Instance.OnEnemyKilled(); // if you use this
            if (destroyOnDeath)
                Destroy(gameObject);
        }
    }
}

