using UnityEngine;
using System.Collections.Generic;

public static class GestureComparer
{
    public static float Compare(List<Vector3> input, List<Vector3> template)
    {
        if (input.Count != template.Count)
        {
            Debug.LogWarning("점 개수가 다릅니다!");
            return 0f;
        }

        float totalDist = 0f;
        for (int i = 0; i < input.Count; i++)
            totalDist += Vector3.Distance(input[i], template[i]);

        float avgDist = totalDist / input.Count;

        return 1f / (1f + avgDist * 10f);
    }
}