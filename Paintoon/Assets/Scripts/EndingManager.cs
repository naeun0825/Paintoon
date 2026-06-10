using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMenu : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Round1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
