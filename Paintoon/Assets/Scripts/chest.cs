using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;

    public GameObject crystalPrefab;
    public Transform spawnPoint;

    public bool hasCrystal = false;
    private bool opened = false;

    public void OpenChest()
    {
        if (opened) return;

        opened = true;

        animator.SetTrigger("open");

        if (hasCrystal)
        {
            GameObject crystal = Instantiate(
                crystalPrefab,
                spawnPoint.position,
                Quaternion.identity
            );

            Destroy(crystal, 3f);
        }
    }
    private void Update()
    {
        // 테스트용
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }
}