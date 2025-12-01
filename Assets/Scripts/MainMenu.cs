using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Names")]
    public string gameSceneName = "GameScene"; // set this in Inspector

    public void PlayGame()
    {
        Time.timeScale = 1f; // just in case it was paused
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();

#if UNITY_EDITOR
        // So it also stops play mode in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
