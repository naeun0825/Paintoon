// AccuracyCanvasFollow.cs
using UnityEngine;

public class AccuracyCanvasFollow : MonoBehaviour
{
    public Camera mainCamera;
    public float distance = 1.5f;

    private void LateUpdate()
    {
        if (!gameObject.activeSelf) return;

        transform.position = mainCamera.transform.position
            + mainCamera.transform.forward * distance;
        transform.rotation = mainCamera.transform.rotation;
    }
}