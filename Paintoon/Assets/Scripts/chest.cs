using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;

    private bool opened = false;

    public void OpenChest()
    {
        if (opened) return;

        opened = true;

        animator.SetTrigger("open");
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