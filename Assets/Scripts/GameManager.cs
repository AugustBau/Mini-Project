using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;          // if using TextMeshPro


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Enemy Settings")]
    public Transform enemySpawnCenter;
    public GameObject enemyPrefab;
    public int initialEnemyCount = 5;
    public float spawnRadius = 2f;

    [Header("Boss Settings")]
    public GameObject bossPrefab;
    public int killsToSpawnBoss = 20;


    [Header("UI")]
    public GameObject winScreen;
    public TextMeshProUGUI killCounterText;

    [Header("Game Over")]
    public GameObject gameOverScreen;        // your GAME OVER panel
    public MonoBehaviour playerController;   // your movement/look script

    int enemiesKilled;
    bool bossSpawned;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (winScreen != null)
        winScreen.SetActive(false);

        SpawnInitialEnemies();
        UpdateKillUI();   // show 0 / 20 at start
    }

    void SpawnInitialEnemies()
    {
        for (int i = 0; i < initialEnemyCount; i++)
        {
            Vector3 pos = enemySpawnCenter.position +
                          Random.insideUnitSphere * spawnRadius;
            pos.y = enemySpawnCenter.position.y;
            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;
        UpdateKillUI();

        if (!bossSpawned && enemiesKilled >= killsToSpawnBoss)
        {
            SpawnBoss();
        }
    }

    void UpdateKillUI()
    {
        if (killCounterText != null)
        {
            killCounterText.text = $"Kills: {enemiesKilled} / {killsToSpawnBoss}";
        }
    }
    void SpawnBoss()
    {
        bossSpawned = true;
        Vector3 pos = enemySpawnCenter.position;
        GameObject boss = Instantiate(bossPrefab, pos, Quaternion.identity);

        Health bossHp = boss.GetComponent<Health>();
        if (bossHp != null)
        {
            bossHp.OnDeath += OnBossKilled;
        }
    }

    void OnBossKilled()
    {
        winScreen.SetActive(true);
        // You can pause time here if you want:
        // Time.timeScale = 0f;
    }
    public void OnPlayerDied()
    {
        // Show Game Over UI
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        // Disable player movement / look
        if (playerController != null)
            playerController.enabled = false;

        // Unlock cursor so you can click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Optional: pause game world
        Time.timeScale = 0f;
    }
}
