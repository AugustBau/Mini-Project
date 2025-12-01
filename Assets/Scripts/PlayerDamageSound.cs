using UnityEngine;

public class PlayerDamageSound : MonoBehaviour
{
    public Health health;            // Player's Health component
    public AudioSource audioSource;  // AudioSource on the player
    public AudioClip damageClip;     // The mp3 you imported

    int lastHealth;

    void Start()
    {
        if (health == null)
            health = GetComponent<Health>();

        if (health != null)
            lastHealth = health.CurrentHealth;
    }

    void Update()
    {
        if (health == null || audioSource == null || damageClip == null)
            return;

        // If health has decreased since last frame, play sound
        if (health.CurrentHealth < lastHealth)
        {
            audioSource.PlayOneShot(damageClip);
        }

        lastHealth = health.CurrentHealth;
    }
}
