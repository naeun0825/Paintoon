using UnityEngine;

[CreateAssetMenu(fileName = "GestureTemplate", menuName = "Magic/GestureTemplate")]
public class GestureTemplate : ScriptableObject
{
    public string magicName;

    [Header("특징 기준값")]
    [Range(0f, 1f)]
    public float minCircularity = 0f;    // 최소 원형도
    [Range(0f, 1f)]
    public float maxCircularity = 1f;    // 최대 원형도
    public int minDirectionChanges = 0;  // 최소 방향 전환 횟수
    public int maxDirectionChanges = 100; // 최대 방향 전환 횟수

    [Header("인식 설정")]
    [Range(0f, 1f)]
    public float minAccuracy = 0.5f;
}