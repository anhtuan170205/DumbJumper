using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
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
    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Audio Settings")]
    [Range(0, 1)] [SerializeField] private float backgroundVolume = 0.5f;
    [Range(0, 1)] [SerializeField] private float soundEffectVolume = 1.0f;
    [Range(1, 100)] [SerializeField] private float dieVolume = 50f;

    private void Start()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        Coin.OnCoinCollected += PlayCoinSound;
        Player.OnPlayerDied += PlayGameOverSound;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.Pause)
        {
            PauseBackgroundMusic();
        }
        else
        {
            ResumeBackgroundMusic();
        }
        if (newState == GameState.GameOver || newState == GameState.MainMenu)
        {
            StopBackgroundMusic();
        }
        if (newState == GameState.InGame)
        {
            PlayBackgroundMusic();
        }
    }

    private void PlayCoinSound()
    {
        AudioSource.PlayClipAtPoint(coinSound, Camera.main.transform.position, soundEffectVolume);
    }

    private void PlayBackgroundMusic()
    {
        if (GetComponent<AudioSource>() != null)
        {
            Destroy(GetComponent<AudioSource>());
        }
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.volume = backgroundVolume;
        audioSource.Play();
    }

    private void PauseBackgroundMusic()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Pause();
        }
    }

    private void ResumeBackgroundMusic()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.UnPause();
        }
    }
    private void PlayGameOverSound()
    {
        Vector3 soundPosition = Camera.main != null ? Camera.main.transform.position : Vector3.zero;
        AudioSource.PlayClipAtPoint(gameOverSound, soundPosition, dieVolume);
    }

    private void StopBackgroundMusic()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Stop();
            Destroy(audioSource);
        }
    }
}
