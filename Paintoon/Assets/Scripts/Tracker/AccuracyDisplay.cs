// AccuracyDisplay.cs
using UnityEngine;
using TMPro;

public class AccuracyDisplay : MonoBehaviour
{
    public TextMeshProUGUI accuracyText;
    public float displayDuration = 2f;

    private float _timer = 0f;
    private bool _isShowing = false;

    private void Start()
    {
        accuracyText.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (_isShowing)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _isShowing = false;
                accuracyText.gameObject.SetActive(false);
            }
        }
    }

    public void ShowAccuracy(float accuracy)
    {
        if (accuracy >= 0.5f) // minAccuracy ±‚¡ÿ
            accuracyText.text = "Magic Ready!";
        else
            accuracyText.text = "Try Again!";

        accuracyText.gameObject.SetActive(true);
        _timer = displayDuration;
        _isShowing = true;
    }
}