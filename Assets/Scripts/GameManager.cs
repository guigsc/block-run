using BlockRun;
using BlockRun.Enum;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject soundButtonCross;

    [SerializeField] UnityEvent onGameOver;

    [SerializeField] Animator sceneTransitionAnimator;
    [SerializeField] Animator volumeTransitionAnimator;
    [SerializeField] float sceneTransitionDuration;

    private Vector3 gravity;

    private GameState gameState;

    private bool mute = false;

    void Start()
    {
        ChangeState(GameState.Menu);
        
        gravity = Physics.gravity;
        Physics.gravity = Vector3.zero;
    }

    public void ChangeState(GameState state)
    {
        gameState = state;
        switch (state)
        {
            case GameState.GameOver:
                StartCoroutine(GameOver());
                break;

            case GameState.Play:
                Play();
                break;

            default:
                break;
        }
    }

    public bool IsGameActive
    {
        get
        {
            return gameState == GameState.Play;
        }
    }

    private void Play()
    {
        Physics.gravity = gravity;
        menuScreen.SetActive(false);
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.2f);
        gameOverScreen.SetActive(true);
        
        if (onGameOver != null)
            onGameOver.Invoke();
    }

    public void RestartGame()
    {
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene()
    {
        sceneTransitionAnimator.SetTrigger("Start");
        volumeTransitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(sceneTransitionDuration);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayGame()
    {
        ChangeState(GameState.Play);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void SoundButtonClick()
    {
        mute = !mute;
        soundButtonCross.SetActive(mute);
        AudioListener.volume = Convert.ToSingle(!mute);
    }
}
