using UnityEngine;

public class MagicGuideDisplay : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Camera _mainCamera;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.01f;
        _lineRenderer.endWidth = 0.01f;

        Color guideColor = new Color(1f, 1f, 1f, 0.3f);
        _lineRenderer.startColor = guideColor;
        _lineRenderer.endColor = guideColor;

        _mainCamera = Camera.main;

        gameObject.SetActive(false); 
    }

    private void LateUpdate()
    {
        if (!gameObject.activeSelf) return;

        transform.position = _mainCamera.transform.position + _mainCamera.transform.forward * 1f + _mainCamera.transform.right * 0.3f;
        transform.rotation = _mainCamera.transform.rotation;
    }

    public void ShowGuide(GestureTemplate template)
    {
        if (template == null || template.points == null) return;

        _lineRenderer.positionCount = template.points.Count;
        for (int i = 0; i < template.points.Count; i++)
            _lineRenderer.SetPosition(i, template.points[i] * 0.15f);

        gameObject.SetActive(true);
    }

    public void HideGuide()
    {
        gameObject.SetActive(false);
    }
}