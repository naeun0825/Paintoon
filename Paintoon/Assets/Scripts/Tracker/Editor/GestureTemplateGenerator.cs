using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(GestureTemplate))]
public class GestureTemplateGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GestureTemplate template = (GestureTemplate)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("템플릿 자동 생성");

        if (GUILayout.Button("Circle"))
            template.points = GenerateCircle(64);

        if (GUILayout.Button("Triangle"))
            template.points = GenerateTriangle(64);

        if (GUILayout.Button("Star"))
            template.points = GenerateStar(64);

        if (GUILayout.Button("StarCircle"))
            template.points = GenerateStarCircle(64);

        if (GUI.changed)
            EditorUtility.SetDirty(template);
    }

    private List<Vector3> GenerateCircle(int count)
    {
        var points = new List<Vector3>();
        for (int i = 0; i < count; i++)
        {
            float angle = (float)i / count * Mathf.PI * 2f;
            points.Add(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f));
        }
        return points;
    }

    private List<Vector3> GenerateTriangle(int count)
    {
        // 삼각형
        Vector3[] corners = new Vector3[]
        {
            new Vector3(0f,  1f, 0f),   
            new Vector3(-1f, -1f, 0f),  
            new Vector3(1f, -1f, 0f),   
        };

        var points = new List<Vector3>();
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

        return points;
    }

    private List<Vector3> GenerateStar(int count)
    {
        var points = new List<Vector3>();
        int tips = 5; // 별

        for (int i = 0; i < count; i++)
        {
            float angle = (float)i / count * Mathf.PI * 2f;
            float r = (i % (count / tips) < (count / tips) / 2) ? 1f : 0.4f;
            points.Add(new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 0f));
        }

        return points;
    }

    private List<Vector3> GenerateStarCircle(int count)
    {
        int half = count / 2;
        var points = new List<Vector3>();

        // 원
        for (int i = 0; i < half; i++)
        {
            float angle = (float)i / half * Mathf.PI * 2f;
            points.Add(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f));
        }

        // 별
        int tips = 5;
        for (int i = 0; i < half; i++)
        {
            float angle = (float)i / half * Mathf.PI * 2f;
            float r = (i % (half / tips) < (half / tips) / 2) ? 1f : 0.4f;
            points.Add(new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r, 0f));
        }

        return GestureNormalizer.Normalize(points);
    }
}