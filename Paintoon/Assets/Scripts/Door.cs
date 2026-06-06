using UnityEngine;


public class Door : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    public float openAngle = 90f;
    public float openSpeed = 2f;

    private bool isOpen = false;

    void Update()
    {
        if (!isOpen) return;

        Quaternion leftTarget = Quaternion.Euler(0, -openAngle, 0);
        Quaternion rightTarget = Quaternion.Euler(0, openAngle, 0);

        leftDoor.localRotation = Quaternion.Slerp(
            leftDoor.localRotation,
            leftTarget,
            Time.deltaTime * openSpeed
        );

        rightDoor.localRotation = Quaternion.Slerp(
            rightDoor.localRotation,
            rightTarget,
            Time.deltaTime * openSpeed
        );
    }

    public void OpenDoor()
    {
        isOpen = true;
        Debug.Log("Door Opening");
    }
}