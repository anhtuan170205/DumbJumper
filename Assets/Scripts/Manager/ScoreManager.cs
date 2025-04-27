using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public static event Action OnScoreChanged;
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
    private int score;
    public int GetScore()
    {
        return score;
    }
    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke();
    }
    public void ResetScore()
    {
        score = 0;
        OnScoreChanged?.Invoke();
    }
}
