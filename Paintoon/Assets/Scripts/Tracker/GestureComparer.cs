using UnityEngine;
using System.Collections.Generic;

public static class GestureComparer
{
    public static float Compare(List<Vector3> input, List<Vector3> template)
    {
        if (input.Count != template.Count)
        {
            return 0f;
        }

        float totalDist = 0f;
        for (int i = 0; i < input.Count; i++)
            totalDist += Vector3.Distance(input[i], template[i]);

        float avgDist = totalDist / input.Count;

        return 1f / (1f + avgDist * 10f);
    }

    // GestureComparer.cs에 추가
    public static float CompareWithRotation(List<Vector3> input, List<Vector3> template)
    {
        float bestScore = 0f;
        int steps = 36; // 10도씩 360도 회전

        for (int i = 0; i < steps; i++)
        {
            float angle = (float)i / steps * 360f;
            List<Vector3> rotated = Rotate(input, angle);
            float score = Compare(rotated, template);
            if (score > bestScore)
                bestScore = score;
        }

        return bestScore;
    }

    private static List<Vector3> Rotate(List<Vector3> points, float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        List<Vector3> result = new List<Vector3>();
        foreach (var p in points)
        {
            float x = p.x * cos - p.y * sin;
            float y = p.x * sin + p.y * cos;
            result.Add(new Vector3(x, y, 0f));
        }
        return result;
    }
}