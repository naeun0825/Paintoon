using UnityEngine;

// Subject 인터페이스
// 정보를 전달하는 주체
public interface Subject
{
    // Observer 등록
    void RegisterObserver(Observer _observer);

    // Observer 해제
    void RemoveObserver(Observer _observer);

    // 모든 Observer 업데이트
    void NotifyObservers();
}

// Observer 인터페이스
// 정보를 받아 갱신하는 관찰자
public interface Observer
{
    // 정보 갱신 및 초기화
    void ObserverUpdate(float healthRatio);
}