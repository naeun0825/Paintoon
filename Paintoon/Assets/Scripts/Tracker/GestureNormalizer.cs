using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class GestureNormalizer
{
    private const int SampleCount = 64; // 몇 개의 점으로 정규화할지

    public static List<Vector3> Normalize(List<Vector3> points)
    {
        List<Vector3> resampled = Resample(points, SampleCount);

        Vector3 centroid = resampled.Aggregate(Vector3.zero, (sum, p) => sum + p) / SampleCount;
        resampled = resampled.Select(p => p - centroid).ToList();

        float maxDist = resampled.Max(p => p.magnitude);
        if (maxDist > 0f)
            resampled = resampled.Select(p => p / maxDist).ToList();

        return resampled;
    }

    private static List<Vector3> Resample(List<Vector3> points, int count)
    {
        float totalLength = 0f;
        for (int i = 1; i < points.Count; i++)
            totalLength += Vector3.Distance(points[i - 1], points[i]);

        float interval = totalLength / (count - 1); // 점 간격
        float accumulated = 0f;

        List<Vector3> result = new List<Vector3> { points[0] };

        for (int i = 1; i < points.Count && result.Count < count; i++)
        {
            float segLen = Vector3.Distance(points[i - 1], points[i]);

            if (accumulated + segLen >= interval)
            {
                float t = (interval - accumulated) / segLen;
                Vector3 newPoint = Vector3.Lerp(points[i - 1], points[i], t);
                result.Add(newPoint);
                points.Insert(i, newPoint);
                accumulated = 0f;
            }
            else
            {
                accumulated += segLen;
            }
        }

        while (result.Count < count)
            result.Add(points[^1]);

        return result;
    }

    public static List<Vector3> ProjectToViewPlane(List<Vector3> points, Camera camera)
    {
        List<Vector3> projected = new List<Vector3>();

        foreach (var point in points)
        {
            // 카메라 로컬 좌표로 변환
            Vector3 localPoint = camera.transform.InverseTransformPoint(point);
            // Z값(깊이)을 0으로 만들어서 평면에 투영
            localPoint.z = 0f;
            projected.Add(localPoint);
        }

        return projected;
    }
}