using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [Header("Canvases")]
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject infoMenu;

    private bool isPaused = false;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // If info is open, go back to pause menu
            if (infoMenu.activeSelf)
            {
                CloseInfo();
                return;
            }

            // If settings is open, go back to pause menu
            if (settingsMenu.activeSelf)
            {
                CloseSettings();
                return;
            }

            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
        infoMenu.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        infoMenu.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        infoMenu.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void OpenInfo()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        infoMenu.SetActive(true);
    }

    public void CloseInfo()
    {
        infoMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
}