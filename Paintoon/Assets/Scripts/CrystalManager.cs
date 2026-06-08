using UnityEngine;
using TMPro;

public class CrystalManager : MonoBehaviour
{
    public int crystalCount = 0;
    public int maxCrystal = 5;

    public TextMeshProUGUI crystalText;

    public GameObject closedDoor;  // ´ÝÈù ¹®
    public GameObject openDoor;    // ¿­¸° ¹®

    void Start()
    {
        UpdateUI();

        if (openDoor != null)
            openDoor.SetActive(false); // ½ÃÀÛÀº ´ÝÈû
    }

    public void AddCrystal()
    {
        crystalCount++;
        UpdateUI();

        if (crystalCount >= maxCrystal)
        {
            OpenDoor();
        }
    }

    void UpdateUI()
    {
        if (crystalText != null)
            crystalText.text = crystalCount + " / " + maxCrystal;
    }

    void OpenDoor()
    {
        Debug.Log("¹® ¿­¸²!");

        if (closedDoor != null)
            closedDoor.SetActive(false);

        if (openDoor != null)
            openDoor.SetActive(true);
    }
}