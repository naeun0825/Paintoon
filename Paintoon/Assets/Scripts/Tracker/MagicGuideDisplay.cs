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

        transform.position = mainCamera.transform.position + mainCamera.transform.forward * 1f + mainCamera.transform.right * 0f;
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
        if (template == null) return;

        // 마법 이름으로 어떤 모양인지 판별
        List<Vector3> guidePoints = GenerateGuidePoints(template.magicName);

        _currentPoints = guidePoints;
        _lineRenderer.positionCount = _currentPoints.Count;
        gameObject.SetActive(true);
    }

    private List<Vector3> GenerateGuidePoints(string magicName)
    {
        int count = 64;
        var points = new List<Vector3>();

        if (magicName == "Circle")
        {
            for (int i = 0; i < count; i++)
            {
                float angle = (float)i / count * Mathf.PI * 2f;
                points.Add(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f));
            }
        }
        else if (magicName == "StarCircle")
        {
            int half = count / 2;
            for (int i = 0; i < half; i++)
            {
                float angle = (float)i / half * Mathf.PI * 2f;
                points.Add(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f));
            }
            int tips = 5;
            for (int i = 0; i < half; i++)
            {
                float angle = (float)i / half * Mathf.PI * 2f;
                float r = (i % (half / tips) < (half / tips) / 2) ? 1f : 0.4f;
                points.Add(new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 0f));
            }
        }

        return points;
    }

    public void HideGuide()
    {
        gameObject.SetActive(false);
    }
}