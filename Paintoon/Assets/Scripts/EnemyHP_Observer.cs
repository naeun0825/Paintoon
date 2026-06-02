using UnityEngine;
using UnityEngine.UI;

public class EnemyHP_Observer : MonoBehaviour, Observer
{
    [SerializeField] private Image hpFillImage;
    [SerializeField] private HP_Subject hP_Subject;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // Subject에 자신을 등록
        if (hP_Subject != null)
            hP_Subject.RegisterObserver(this);
    }

    // 옵저버 업데이트 호출 (HP_Subject에서 Notify할 때 실행됨)
    public void ObserverUpdate(float healthRatio)
    {
        if (hpFillImage != null)
        {
            hpFillImage.fillAmount = healthRatio;
        }
    }

    void LateUpdate()
    {
        // UI가 항상 카메라를 바라보게 함 (빌보드 효과)
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
    }
}
