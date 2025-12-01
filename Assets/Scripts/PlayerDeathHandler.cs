using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    public Health playerHealth;
    public GameObject gameOverPanel;

    void Start()
    {
        if (playerHealth == null)
            playerHealth = GetComponent<Health>();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Subscribe to death event
        playerHealth.OnDeath += OnPlayerDied;
    }

    void OnPlayerDied()
    {
        // Show Game Over UI
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Unlock cursor so player can click menu etc.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Optional: freeze game
        Time.timeScale = 0f;
    }
}
