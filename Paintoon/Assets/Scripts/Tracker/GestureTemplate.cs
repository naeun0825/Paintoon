using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GestureTemplate", menuName = "Magic/GestureTemplate")]
public class GestureTemplate : ScriptableObject
{
    public string magicName;        
    public List<Vector3> points;    
    [Range(0f, 1f)]
    public float minAccuracy = 0.5f; 
}