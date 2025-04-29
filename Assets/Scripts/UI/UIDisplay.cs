using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDisplay : MonoBehaviour
{
    [Header("Score Display")]
    [SerializeField] private TextMeshProUGUI scoreText;
    
    [Header("Grapple Cooldown Display")]
    [SerializeField] private Slider grappleCooldownSlider;
    [SerializeField] private GrapplingHook grapplingHook; 
    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += UpdateScoreDisplay;
        
        if (grappleCooldownSlider != null)
        {
            grappleCooldownSlider.minValue = 0;
            grappleCooldownSlider.maxValue = 1;
            grappleCooldownSlider.value = 1; // Start fully charged
        }
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= UpdateScoreDisplay;
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = ScoreManager.Instance.GetScore().ToString("00");
        }
    }

    private void Update()
    {
        UpdateGrappleCooldownSlider();
    }

    private void UpdateGrappleCooldownSlider()
    {
        if (grapplingHook == null || grappleCooldownSlider == null) return;
        float cooldownProgress = (grapplingHook.CurrentCooldown / grapplingHook.GrappleCooldown);
        grappleCooldownSlider.value = cooldownProgress;
    }
}