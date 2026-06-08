using UnityEngine;
using TMPro;

public class WristUIController : MonoBehaviour
{
    [Header("참조")]
    public Transform leftController;
    public GameObject wristUI;
    public TextMeshProUGUI hpText;

    [Header("설정")]
    [Range(0f, 1f)]
    public float activationThreshold = 0.9f; // 손등이 위를 향하는 정도

    private void Start()
    {
        // PlayerHealth 이벤트 구독
        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.OnHealthChanged += UpdateHP;
            UpdateHP(PlayerHealth.Instance.maxHealth); // 초기 HP 표시
        }
    }

    private void OnDestroy()
    {
        if (PlayerHealth.Instance != null)
            PlayerHealth.Instance.OnHealthChanged -= UpdateHP;
    }

    private void Update()
    {
        // 손등이 위를 향하는지 확인
        // Left Controller의 Up 벡터와 세계 Up 벡터의 내적
        float dot = Vector3.Dot(leftController.up, Vector3.up);
        wristUI.transform.rotation = leftController.rotation * Quaternion.Euler(0, 90, 90);

        if (dot < -activationThreshold)
            wristUI.SetActive(true);
        else
            wristUI.SetActive(false);
    }

    // 외부에서 HP 업데이트할 때 호출
    public void UpdateHP(int hp)
    {
        hpText.text = $"HP: {hp}";
    }
}
