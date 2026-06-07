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

            // 원 파트
            for (int i = 0; i < half; i++)
            {
                float angle = (float)i / half * Mathf.PI * 2f;
                points.Add(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f));
            }

            // 별 파트 (뾰족한 별)
            Vector3[] starPoints = new Vector3[10];
            for (int i = 0; i < 10; i++)
            {
                float angle = (float)i / 10 * Mathf.PI * 2f - Mathf.PI / 2f;
                float r = (i % 2 == 0) ? 1f : 0.4f;
                starPoints[i] = new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 0f);
            }

            int perSegment = half / 10;
            for (int s = 0; s < 10; s++)
            {
                Vector3 from = starPoints[s];
                Vector3 to = starPoints[(s + 1) % 10];
                for (int i = 0; i < perSegment; i++)
                {
                    float t = (float)i / perSegment;
                    points.Add(Vector3.Lerp(from, to, t));
                }
            }

            while (points.Count < count)
                points.Add(starPoints[0]);

        }
        else if (magicName == "Star")
        {
            int tips = 5;
            // 꼭짓점 10개 (바깥 5개, 안쪽 5개) 먼저 계산
            Vector3[] starPoints = new Vector3[10];
            for (int i = 0; i < 10; i++)
            {
                float angle = (float)i / 10 * Mathf.PI * 2f - Mathf.PI / 2f;
                float r = (i % 2 == 0) ? 1f : 0.4f; // 짝수: 바깥, 홀수: 안쪽
                starPoints[i] = new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 0f);
            }

            // 꼭짓점 사이를 균등하게 채우기
            int perSegment = count / 10;
            for (int s = 0; s < 10; s++)
            {
                Vector3 from = starPoints[s];
                Vector3 to = starPoints[(s + 1) % 10];
                for (int i = 0; i < perSegment; i++)
                {
                    float t = (float)i / perSegment;
                    points.Add(Vector3.Lerp(from, to, t));
                }
            }

            while (points.Count < count)
                points.Add(starPoints[0]);
        }
        else if (magicName == "Triangle")
        {
            Vector3[] corners = new Vector3[]
            {
                new Vector3(0f, 1f, 0f),
                new Vector3(-1f, -1f, 0f),
                new Vector3(1f, -1f, 0f),
            };
            int perSide = count / 3;
            for (int s = 0; s < 3; s++)
            {
                Vector3 from = corners[s];
                Vector3 to = corners[(s + 1) % 3];
                for (int i = 0; i < perSide; i++)
                {
                    float t = (float)i / perSide;
                    points.Add(Vector3.Lerp(from, to, t));
                }
            }
            while (points.Count < count)
                points.Add(corners[0]);
        }

        return points;
    }

    public void HideGuide()
    {
        gameObject.SetActive(false);
    }
}