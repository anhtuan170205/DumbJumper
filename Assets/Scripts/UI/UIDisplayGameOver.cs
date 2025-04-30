using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDisplayGameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        UpdateScoreDisplay();
    }
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = ScoreManager.Instance.GetScore().ToString("00");
        }
    }
}
