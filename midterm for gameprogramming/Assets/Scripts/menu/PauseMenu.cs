using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;

    [Header("First Selected")]
    [SerializeField] private GameObject pauseMenuFirst;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
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
}
