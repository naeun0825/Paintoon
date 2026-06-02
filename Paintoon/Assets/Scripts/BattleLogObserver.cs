using UnityEngine;

public class BattleLog_Observer : MonoBehaviour, Observer
{
    [SerializeField] private HP_Subject hP_Subject;

    void Start()
    {
        // Subject에 자신을 등록
        if (hP_Subject != null)
            hP_Subject.RegisterObserver(this);
    }

    public void ObserverUpdate(float healthRatio)
    {
        string unitName = gameObject.name;

        if (healthRatio <= 0)
        {
            Debug.Log($"<color=red>[사망 소식]</color> {unitName}이(가) 전사했습니다.");
        }
        else
        {
            Debug.Log($"[전투 알림] {unitName}의 현재 체력: {healthRatio}");
        }
    }
}