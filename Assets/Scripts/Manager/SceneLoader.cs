using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadGameplay()
    {
        SceneManager.LoadScene("Gameplay");
        ScoreManager.Instance.ResetScore(); // Make sure ScoreManager is still valid
        Time.timeScale = 1f;
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
