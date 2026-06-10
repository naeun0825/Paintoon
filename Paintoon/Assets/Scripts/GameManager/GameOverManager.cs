// GameOverManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public Camera mainCamera;
    public float distance = 1.5f;

    private bool _isGameOver = false;
    public static GameOverManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

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
        SceneManager.LoadScene("Round1"); // 첫 번째 씬 이름
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.OnPlayerDeath += ShowGameOver;
        }
    }

    private void OnDestroy()
    {
        if (PlayerHealth.Instance != null)
            PlayerHealth.Instance.OnPlayerDeath -= ShowGameOver;
    }
}