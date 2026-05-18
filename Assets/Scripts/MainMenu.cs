using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Names")]
    public string gameSceneName = "Level_01_NIght";
    public string mainMenuSceneName = "MainMenu";

    [Header("Music")]
    public AudioClip backgroundMusic;

    private void Start()
    {
        // Make sure SoundManager exists
        if (SoundManager.Instance != null)
        {
            // Only start music if nothing is already playing
            if (!SoundManager.Instance.ambientSource.isPlaying)
            {
                SoundManager.Instance.PlayAmbient(backgroundMusic);
            }
        }
        else
        {
            Debug.LogWarning("SoundManager instance not found.");
        }
    }

    // Start the game
    public void StartGame()
    {
        Debug.Log("StartGame button pressed.");

        SceneManager.LoadScene(gameSceneName);

        Debug.Log("Scene load command sent.");
    }

    // Return to main menu
    public void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu.");

        SceneManager.LoadScene(mainMenuSceneName);

        Debug.Log("Main Menu load command sent.");
    }

    // Restart the game scene
    public void RestartGame()
    {
        Debug.Log("Restarting game.");

        SceneManager.LoadScene(gameSceneName);

        Debug.Log("Restart load command sent.");
    }

    // Quit game
    public void QuitGame()
    {
        Debug.Log("QuitGame button pressed.");

        Application.Quit();
    }
}