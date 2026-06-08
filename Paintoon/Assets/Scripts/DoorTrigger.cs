using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
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
        Debug.Log("Scene Loading...");
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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