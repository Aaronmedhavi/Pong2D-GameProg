using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameManager gameManager; // Reference to your GameManager script
    public SceneManagement sceneManagement; // Reference to your existing SceneManagement script

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause game time
        GameIsPaused = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Ensure the game is unpaused
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Resume time to ensure everything runs correctly in the main menu
        sceneManagement.ChangeScene("MainMenu"); // Use your SceneManagement script to change scenes
    }
}
