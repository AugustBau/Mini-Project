using UnityEngine;
using UnityEngine.SceneManagement;

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
        winScreen.SetActive(false);
        SpawnInitialEnemies();
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

        if (!bossSpawned && enemiesKilled >= killsToSpawnBoss)
        {
            SpawnBoss();
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
}
