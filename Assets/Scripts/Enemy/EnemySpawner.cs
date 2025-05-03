using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("Enemy Prefab")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn Corners")]
    [SerializeField] private Vector2 topLeft;
    [SerializeField] private Vector2 topRight;
    [SerializeField] private Vector2 bottomLeft;
    [SerializeField] private Vector2 bottomRight;

    private float spawnInterval = 5f;
    private float spawnTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ScoreManager.OnXScoreChanged += HandleXScoreChanged;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        int direction = Random.Range(0, 4); // 0 = Top, 1 = Bottom, 2 = Left, 3 = Right
        Vector2 spawnPos = Vector2.zero;

        switch (direction)
        {
            case 0: spawnPos = RandomInRange(topLeft, topRight); break;        // Top
            case 1: spawnPos = RandomInRange(bottomLeft, bottomRight); break;  // Bottom
            case 2: spawnPos = RandomInRange(bottomLeft, topLeft); break;      // Left
            case 3: spawnPos = RandomInRange(bottomRight, topRight); break;    // Right
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        if (enemy.TryGetComponent<Enemy>(out var enemyScript))
        {
            switch (direction)
            {
                case 0: enemyScript.Initialize(Enemy.SpawnSide.Top); break;
                case 1: enemyScript.Initialize(Enemy.SpawnSide.Bottom); break;
                case 2: enemyScript.Initialize(Enemy.SpawnSide.Left); break;
                case 3: enemyScript.Initialize(Enemy.SpawnSide.Right); break;
            }
        }
    }

    private Vector2 RandomInRange(Vector2 a, Vector2 b)
    {
        float minX = Mathf.Min(a.x, b.x);
        float maxX = Mathf.Max(a.x, b.x);
        float minY = Mathf.Min(a.y, b.y);
        float maxY = Mathf.Max(a.y, b.y);

        return new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );
    }

    private void HandleXScoreChanged()
    {
        spawnInterval = Mathf.Max(1f, spawnInterval - 0.5f);
    }
}
