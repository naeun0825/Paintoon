using UnityEngine;
using TMPro;

public class WristUIController : MonoBehaviour
{
    [Header("참조")]
    public Transform leftController;
    public GameObject wristUI;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI timeText;

    [Header("설정")]
    [Range(0f, 1f)]
    public float activationThreshold = 0.7f; // 손등이 위를 향하는 정도

    private float _remainingTime = 60f;
    private bool _isRunning = true;

    private void Update()
    {
        // 손등이 위를 향하는지 확인
        // Left Controller의 Up 벡터와 세계 Up 벡터의 내적
        float dot = Vector3.Dot(leftController.up, Vector3.up);

        if (dot < -activationThreshold)
            wristUI.SetActive(true);
        else
            wristUI.SetActive(false);

        // 시간 업데이트
        if (_isRunning)
        {
            //_remainingTime -= Time.deltaTime;
            //if (_remainingTime <= 0f)
            //{
            //    _remainingTime = 0f;
            //    _isRunning = false;
            //}
            //timeText.text = $"Time: {Mathf.CeilToInt(_remainingTime)}";
        }
    }

    // 외부에서 HP 업데이트할 때 호출
    public void UpdateHP(int hp)
    {
        hpText.text = $"HP: {hp}";
    }
}
