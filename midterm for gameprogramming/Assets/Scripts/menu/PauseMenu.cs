using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;

    public GameObject gameOverScreen;

    [Header("First Selected")]
    [SerializeField] private GameObject pauseMenuFirst;
    [SerializeField] private GameObject gameOverScreenFirst;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        gameOverScreen.SetActive(false);
        PlayerHealth.OnPlayerDied += GameOverScreen;
        LevelEnd.LoadLevel += LoadLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.MenuOpenInput)
        {
            if (!isPaused)
            {
                PauseGame();
            }
        }
        else if (InputManager.MenuCloseInput)
        {
            if (isPaused)
            {
                ResumeGame();
            }
        }
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        EventSystem.current.SetSelectedGameObject(pauseMenuFirst);

        InputManager.PlayerInput.SwitchCurrentActionMap("UI");
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        EventSystem.current.SetSelectedGameObject(null);

        InputManager.PlayerInput.SwitchCurrentActionMap("Player");
    }
    public void backToMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("game was succesfully closed from pause");
    }

    void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        EventSystem.current.SetSelectedGameObject(gameOverScreenFirst);

        InputManager.PlayerInput.SwitchCurrentActionMap("UI");
    }

    void LoadLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "level1")
        {
            SceneManager.LoadScene("level2");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ResetGame()
    {
        gameOverScreenFirst.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        EventSystem.current.SetSelectedGameObject(null);

        InputManager.PlayerInput.SwitchCurrentActionMap("Player");

        SceneManager.LoadScene(currentScene);
    }
}
