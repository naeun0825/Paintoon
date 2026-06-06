using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMenu : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("R1");
    }

    public void ExitGame()
    {
        Application.Quit();

        Debug.Log("게임 종료");
    }
}
