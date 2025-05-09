using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public static event Action OnScoreChanged;
    public static event Action OnXScoreChanged;

    [SerializeField] private int scoreLevel = 10;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        if (score % scoreLevel == 0)
        {
            OnXScoreChanged?.Invoke();
        }
    }
    public void ResetScore()
    {
        score = 0;
        OnScoreChanged?.Invoke();
    }
}
