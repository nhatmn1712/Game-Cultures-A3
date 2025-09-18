using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Play
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Diablo");          // or your gameplay scene name
    }

    // Retry
    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Grandma");     // your gameplay scene name
    }

    // Back to Main Menu
    public void MainMenuGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
