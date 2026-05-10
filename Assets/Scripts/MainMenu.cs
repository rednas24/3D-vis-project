using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Name of your game scene
    public string gameSceneName = "Level_01_NIght";

    // Name of your main menu scene
    public string mainMenuSceneName = "MainMenu";

    // Start the game
    public void StartGame()
    {
        Debug.Log("StartGame button pressed.");

        Debug.Log("Attempting to load scene: " + gameSceneName);

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