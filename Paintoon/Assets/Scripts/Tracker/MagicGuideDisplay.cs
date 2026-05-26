using UnityEngine;

public class MagicGuideDisplay : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.01f;
        _lineRenderer.endWidth = 0.01f;

        Color guideColor = new Color(1f, 1f, 1f, 0.5f);
        _lineRenderer.startColor = guideColor;
        _lineRenderer.endColor = guideColor;

        gameObject.SetActive(false); 
    }

    public void ShowGuide(GestureTemplate template)
    {
        if (template == null || template.points == null) return;

        _lineRenderer.positionCount = template.points.Count;
        for (int i = 0; i < template.points.Count; i++)
            _lineRenderer.SetPosition(i, template.points[i] * 0.2f);

        gameObject.SetActive(true);
    }

    public void HideGuide()
    {
        gameObject.SetActive(false);
    }
}