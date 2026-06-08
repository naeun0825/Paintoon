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

    public static List<Vector3> ProjectToBestPlane(List<Vector3> points)
    {
        if (points.Count < 3) return points;

        // 1. 중심점 계산
        Vector3 centroid = Vector3.zero;
        foreach (var p in points)
            centroid += p;
        centroid /= points.Count;

        // 2. 공분산 행렬 계산
        float xx = 0, xy = 0, xz = 0, yy = 0, yz = 0, zz = 0;
        foreach (var p in points)
        {
            Vector3 r = p - centroid;
            xx += r.x * r.x;
            xy += r.x * r.y;
            xz += r.x * r.z;
            yy += r.y * r.y;
            yz += r.y * r.z;
            zz += r.z * r.z;
        }

        // 3. 가장 잘 맞는 평면의 법선 벡터 찾기
        Vector3 normal = FindBestNormal(xx, xy, xz, yy, yz, zz);

        // 4. 평면의 두 축 계산
        Vector3 axisX = Vector3.Cross(normal, Vector3.up).normalized;
        if (axisX.magnitude < 0.01f)
            axisX = Vector3.Cross(normal, Vector3.forward).normalized;
        Vector3 axisY = Vector3.Cross(normal, axisX).normalized;

        // 5. 평면에 투영
        List<Vector3> projected = new List<Vector3>();
        foreach (var p in points)
        {
            Vector3 r = p - centroid;
            float x = Vector3.Dot(r, axisX);
            float y = Vector3.Dot(r, axisY);
            projected.Add(new Vector3(x, y, 0f));
        }

        return projected;
    }

    private static Vector3 FindBestNormal(float xx, float xy, float xz, float yy, float yz, float zz)
    {
        // 가장 작은 분산 방향이 법선
        Vector3 nX = new Vector3(xx, xy, xz);
        Vector3 nY = new Vector3(xy, yy, yz);
        Vector3 nZ = new Vector3(xz, yz, zz);

        float lenX = nX.magnitude;
        float lenY = nY.magnitude;
        float lenZ = nZ.magnitude;

        if (lenX <= lenY && lenX <= lenZ)
            return nX.normalized;
        else if (lenY <= lenX && lenY <= lenZ)
            return nY.normalized;
        else
            return nZ.normalized;
    }
}