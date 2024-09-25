using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        gameObject.SetActive(false);
        FindFirstObjectByType<FPSController>().StartGame();
    }

    public void QuitGame()
    {
        // Exit the game
        Application.Quit();
    }
    public void Settings()
    {
    }
}

