using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState
{
    MainMenu,
    InGame,
    Pause,
    GameOver
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    public static GameManager Instance { get; private set; }

    public static GameState CurrentGameState { get; private set; }

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        inputReader.PauseEvent += HandlePause;
        CurrentGameState = GameState.MainMenu;
        OnGameStateChanged?.Invoke(CurrentGameState);

        // Optional: if using player death event
        Player.OnPlayerDied += HandlePlayerDied;
    }

    private void OnDestroy()
    {
        inputReader.PauseEvent -= HandlePause;
        Player.OnPlayerDied -= HandlePlayerDied;
    }

    public static void SetGameState(GameState newState)
    {
        if (CurrentGameState == newState) return;

        CurrentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandlePause()
    {
        if (CurrentGameState == GameState.InGame)
        {
            PauseGame();
        }
        else if (CurrentGameState == GameState.Pause)
        {
            ResumeGame();
        }
    }

    private void HandlePlayerDied()
    {
        StartCoroutine(HandlePlayerDeathWithDelay());
    }

    private IEnumerator HandlePlayerDeathWithDelay()
    {
        SetGameState(GameState.GameOver);
        yield return new WaitForSecondsRealtime(0.3f); // let sound play
        SceneLoader.LoadGameOver();
    }


    public void StartGame()
    {
        SetGameState(GameState.InGame);
        Time.timeScale = 1f;
        SceneLoader.LoadGameplay();
    }

    public void LoadMainMenu()
    {
        SetGameState(GameState.MainMenu);
        Time.timeScale = 1f;
        SceneLoader.LoadMainMenu();
    }

    public void QuitGame()
    {
        SceneLoader.QuitGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        SetGameState(GameState.Pause);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        SetGameState(GameState.InGame);
    }
}
