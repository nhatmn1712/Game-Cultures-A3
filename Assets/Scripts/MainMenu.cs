using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void RetryGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void MainMenuGame()
    {
        SceneManager.LoadSceneAsync(0);
    }



    public void QuitGame()
    {
        Application.Quit();
    }

}
