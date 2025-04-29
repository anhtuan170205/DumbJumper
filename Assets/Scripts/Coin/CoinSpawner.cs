using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public static CoinSpawner Instance { get; private set; }

    [SerializeField] private Coin coinPrefab;
    [SerializeField] private int maxCoins = 10;
    [SerializeField] private Vector2 xSpawnRange;
    [SerializeField] private Vector2 ySpawnRange;
    [SerializeField] private LayerMask layerMask;

    private Coin[] coins;
    private float coinRadius;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        coinRadius = coinPrefab.GetComponent<CircleCollider2D>().radius;
        coins = new Coin[maxCoins];

        for (int i = 0; i < maxCoins; i++)
        {
            Vector2 spawnPoint = GetSpawnPoint();
            Coin newCoin = Instantiate(coinPrefab, spawnPoint, Quaternion.identity);
            newCoin.transform.SetParent(this.transform);
            newCoin.gameObject.SetActive(true);
            coins[i] = newCoin;
        }
    }

    private Vector2 GetSpawnPoint()
    {
        while (true)
        {
            float x = Random.Range(xSpawnRange.x, xSpawnRange.y);
            float y = Random.Range(ySpawnRange.x, ySpawnRange.y);
            Vector2 point = new Vector2(x, y);
            if (Physics2D.OverlapCircle(point, coinRadius, layerMask) == null)
            {
                return point;
            }
        }
    }

    public void RespawnCoin(Coin coin)
    {
        StartCoroutine(RespawnAfterDelay(coin));
    }

    private IEnumerator RespawnAfterDelay(Coin coin)
    {
        coin.gameObject.SetActive(false);
        float delay = Random.Range(2f, 5f);
        yield return new WaitForSeconds(delay);

        Vector2 newSpawn = GetSpawnPoint();
        coin.transform.position = newSpawn;
        coin.gameObject.SetActive(true);
    }
}
