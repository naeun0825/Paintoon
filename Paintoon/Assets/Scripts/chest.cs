using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;

    public GameObject crystalPrefab;
    public Transform spawnPoint;

    public bool hasCrystal = false;
    private bool opened = false;

    public CrystalManager crystalManager;

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
            CrystalItem item = crystal.GetComponent<CrystalItem>();

            if (item != null)
            {
                item.SetManager(crystalManager);
            }
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