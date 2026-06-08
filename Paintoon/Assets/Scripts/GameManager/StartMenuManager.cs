// StartMenuManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public GameObject startCanvas;
    public GameObject helpCanvas; // 도움말 UI (나중에 만들 것)
    public Camera mainCamera;
    public float distance = 1.5f;

    private void Start()
    {
        // 게임 시작 시 StartCanvas를 카메라 정면에 배치
        startCanvas.transform.position = mainCamera.transform.position
            + mainCamera.transform.forward * distance;
        startCanvas.transform.rotation = mainCamera.transform.rotation;

        startCanvas.SetActive(true);
    }

    public void OnStartButton()
    {
        startCanvas.SetActive(false);
        GameStateManager.Instance.StartGame();
    }

    public void OnHelpButton()
    {
        helpCanvas.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;
        helpCanvas.transform.rotation = mainCamera.transform.rotation;
        startCanvas.SetActive(false);
        helpCanvas.SetActive(true);
    }

    public void OnCloseHelpButton()
    {
        helpCanvas.SetActive(false);
        startCanvas.SetActive(true);
    }

    public void OnQuitButton()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }
}