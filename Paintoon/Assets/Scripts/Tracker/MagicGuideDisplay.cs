using UnityEngine;
using System.Collections.Generic;

public class MagicGuideDisplay : MonoBehaviour
{
    private List<Vector3> _currentPoints;
    private LineRenderer _lineRenderer;
    public Camera mainCamera;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.01f;
        _lineRenderer.endWidth = 0.01f;
        _lineRenderer.useWorldSpace = true;

        Color guideColor = new Color(1f, 1f, 1f, 0.3f);
        _lineRenderer.startColor = guideColor;
        _lineRenderer.endColor = guideColor;

        gameObject.SetActive(false); 
    }

    private void LateUpdate()
    {
        if (!gameObject.activeSelf) return;

        transform.position = mainCamera.transform.position + mainCamera.transform.forward * 1f + mainCamera.transform.right * 0.3f;
        transform.rotation = mainCamera.transform.rotation;

        if (_currentPoints != null)
        {
            for (int i = 0; i < _currentPoints.Count; i++)
            {
                Vector3 worldPos = transform.TransformPoint(_currentPoints[i] * 0.2f);
                _lineRenderer.SetPosition(i, worldPos);
            }
        }
    }

    public void ShowGuide(GestureTemplate template)
    {

        if (template == null || template.points == null) {
            return;
        }

        _currentPoints = template.points;

        _lineRenderer.positionCount = template.points.Count;

        gameObject.SetActive(true);;
    }

    public void HideGuide()
    {
        gameObject.SetActive(false);
    }
}