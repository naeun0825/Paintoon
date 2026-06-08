using UnityEngine;
using System.Collections.Generic;

public static class GestureAnalyzer
{
    public static float Analyze(List<Vector3> normalized, GestureTemplate template)
    {
        // 1. 원형도 계산
        float circularity = CalculateCircularity(normalized);

        // 2. 방향 전환 횟수 계산
        int directionChanges = CalculateDirectionChanges(normalized);

        Debug.Log($"원형도: {circularity:F2}, 방향전환: {directionChanges}");

        // 3. 템플릿 기준값과 비교
        float circularityScore = 1f - Mathf.Abs(circularity -
            (template.minCircularity + template.maxCircularity) / 2f);

        bool directionInRange = directionChanges >= template.minDirectionChanges
            && directionChanges <= template.maxDirectionChanges;

        float directionScore = directionInRange ? 1f : 0.3f;

        return circularityScore * directionScore;
    }

    // 원형도: 점들이 중심에서 얼마나 일정한 거리인지 (1에 가까울수록 원)
    private static float CalculateCircularity(List<Vector3> points)
    {
        // 평균 거리 계산
        float avgDist = 0f;
        foreach (var p in points)
            avgDist += p.magnitude;
        avgDist /= points.Count;

        // 거리 편차 계산
        float variance = 0f;
        foreach (var p in points)
            variance += Mathf.Abs(p.magnitude - avgDist);
        variance /= points.Count;

        // 편차가 작을수록 원에 가까움
        return 1f / (1f + variance * 5f);
    }

    // 방향 전환 횟수: 궤적이 꺾이는 횟수
    private static int CalculateDirectionChanges(List<Vector3> points)
    {
        int changes = 0;
        float threshold = 0.3f; // 이 각도 이상 꺾여야 전환으로 인식

        for (int i = 1; i < points.Count - 1; i++)
        {
            Vector3 prev = (points[i] - points[i - 1]).normalized;
            Vector3 next = (points[i + 1] - points[i]).normalized;
            float dot = Vector3.Dot(prev, next);

            if (dot < threshold)
                changes++;
        }

        return changes;
    }
}