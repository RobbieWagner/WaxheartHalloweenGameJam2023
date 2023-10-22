using System.Collections;
using System.Collections.Generic;
using RobbieWagnerGames;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SimplePauseMenu : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [HideInInspector] public bool paused = false;
    private PlayerInputActions actions;

    public static SimplePauseMenu Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 

        actions = new PlayerInputActions();
        canvas.enabled = false;
        paused = false;
        Time.timeScale = 1; 
        //GameSounds.Instance.ToggleMute(false);
}

    private void OnPause(InputValue value)
    {
        if(paused)
        {
            canvas.enabled = false;
            paused = false;
            Time.timeScale = 1;
            //GameSounds.Instance.ToggleMute(false);
        }
        else
        {
            canvas.enabled = true;
            paused = true;
            Time.timeScale = 0;
            //GameSounds.Instance.ToggleMute(true);
        }

        if(paused) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
        OnPauseGame?.Invoke(paused);
    }
    public delegate void OnPauseDelegate(bool isPaused);
    public event OnPauseDelegate OnPauseGame;

    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void TogglePause()
    {
        OnPause(null);
    }
}
