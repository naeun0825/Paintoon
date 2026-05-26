using UnityEngine;
using System.Collections.Generic;

public class GestureRecorder : MonoBehaviour
{
    public Transform rightHandTransform;

    private List<Vector3> _points = new List<Vector3>();
    private bool _isRecording = false;

    public void StartRecording()
    {
        _points.Clear();
        _isRecording = true;
    }

    public List<Vector3> StopRecording()
    {
        _isRecording = false;
        return new List<Vector3>(_points); 
    }

    public void AddPoint(Vector3 point)
    {
        if (_isRecording)
            _points.Add(point);
    }

    private void Update()
    {
        if (_isRecording)
            _points.Add(rightHandTransform.position);
    }
}