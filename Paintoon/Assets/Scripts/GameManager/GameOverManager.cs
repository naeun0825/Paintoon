// GameOverManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public Camera mainCamera;
    public float distance = 1.5f;

    private bool _isGameOver = false;

    public void ShowGameOver()
    {
        if (_isGameOver) return;
        _isGameOver = true;

        GameStateManager.Instance.GameOver();

        // 카메라 정면에 위치 고정
        gameOverCanvas.transform.position = mainCamera.transform.position
            + mainCamera.transform.forward * distance;
        gameOverCanvas.transform.rotation = mainCamera.transform.rotation;

        gameOverCanvas.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("R1"); // 첫 번째 씬 이름
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }

    private void Start()
    {
        if (PlayerHealth.Instance != null)
        {
            Debug.Log("PlayerHealth 연결");
            PlayerHealth.Instance.OnPlayerDeath += ShowGameOver;
        }
        else
        {
            Debug.Log("실패");
        }
    }

    private void OnDestroy()
    {
        if (PlayerHealth.Instance != null)
            PlayerHealth.Instance.OnPlayerDeath -= ShowGameOver;
    }
}