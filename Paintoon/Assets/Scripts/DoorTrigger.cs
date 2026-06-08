using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{

    public GameObject endingCanvas;
    public Camera mainCamera;
    public float distance = 1.5f;


    public string nextSceneName = "R2";

    public CrystalManager crystalManager; 

    private bool canEnter = false;

    void Update()
    {
        if (canEnter)
        {
            Debug.Log("canEnter true");

            Debug.Log("crystal: " +
                crystalManager.crystalCount + " / " +
                crystalManager.maxCrystal);
        }
    }

    public void LoadNextScene()
    {
        if (nextSceneName == "Ending" || SceneManager.GetActiveScene().name == "Round3")
        {
            if(endingCanvas != null) {
                endingCanvas.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;
                endingCanvas.transform.rotation = mainCamera.transform.rotation;
                endingCanvas.SetActive(true);
                GameStateManager.Instance.GameOver();
            }
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Player"))
        {
            if (GameStateManager.Instance == null || !GameStateManager.Instance.isGameStarted) return;

            Debug.Log("Player near door");
            if (crystalManager != null && crystalManager.crystalCount >= crystalManager.maxCrystal)
            {
                LoadNextScene();
            }
            else
            {
                Debug.Log("크리스탈이 부족합니다!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canEnter = false;
        }
    }
}