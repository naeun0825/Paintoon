// GameStateManager.cs
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public bool isGameStarted = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartGame()
    {
        isGameStarted = true;
    }

    public void GameOver()
    {
        isGameStarted = false;
    }
}