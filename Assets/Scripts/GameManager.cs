using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    internal bool isGameOver;
    internal bool isGamePaused;
    internal bool isWinner;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        
    }

    internal void GameOver()
    {
        if(isGameOver && isWinner)
        {
            WinScreen();
        }
        else if(isGameOver && !isWinner)
        {
            LoseScreen();
        }
    }
    void WinScreen()
    {
        SceneManager.LoadScene(3);
        Cursor.visible = true;
        Cursor.lockState= CursorLockMode.None;
    }
    void LoseScreen()
    {
        SceneManager.LoadScene(4);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Rematch()
    {
        SceneManager.LoadScene(2);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void HowToPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void Level_1()
    {
        SceneManager.LoadScene(2);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
