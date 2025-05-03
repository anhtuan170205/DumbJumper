using UnityEngine;
using System;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;
    public static event Action OnCoinCollected;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnCoinCollected?.Invoke();
            CoinSpawner.Instance.RespawnCoin(this);
            ScoreManager.Instance.AddScore(coinValue);
        }
    }
}
