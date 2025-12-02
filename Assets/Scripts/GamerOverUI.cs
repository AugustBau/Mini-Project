using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("Scenes")]
    public string mainMenuSceneName = "MainMenu"; // set this in Inspector

    public void BackToTitle()
    {
        // In case you paused the game on death
        Time.timeScale = 1f;

        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
