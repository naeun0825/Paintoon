using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    // 게임 전체에서 접근할 수 있는 싱글톤 인스턴스
    public static TimerManager Instance { get; private set; }

    // 게임 제한 시간 설정 (120초)
    public float gameTime = 120f;

    // 게임 종료 여부를 체크하는 변수
    private bool isGameOver = false;

    public TextMeshProUGUI timeText;

    void Awake()
    {
        // 싱글톤 패턴 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 게임 오버 상태라면 타이머 로직을 더 이상 실행하지 않음
        if (isGameOver) return;

        if (GameStateManager.Instance == null || !GameStateManager.Instance.isGameStarted) return;

        // 시간이 남아있다면 매 프레임마다 남은 시간에서 deltaTime을 뺌
        if (gameTime > 0)
        {
            gameTime -= Time.deltaTime;
            UpdateTimeUI();
        }
        else
        {
            // 시간이 0 이하가 되면 정확히 0으로 맞추고 게임 오버 함수 실행
            gameTime = 0;
            UpdateTimeUI();
            EndGame();
        }
    }

    // UI 텍스트에 남은 시간을 표시해주는 함수
    void UpdateTimeUI()
    {
        // timeText가 인스펙터에 잘 연결되어 있을 때만 실행하여 에러를 방지합니다.
        if (timeText != null)
        {

            int minutes = Mathf.FloorToInt(gameTime / 60);
            int seconds = Mathf.FloorToInt(gameTime % 60);
            timeText.text = "\nTime : " + string.Format("{0:00}:{1:00}", minutes, seconds);

        }
    }

    void EndGame()
    {
        isGameOver = true;
        Debug.Log("120초가 모두 지나 게임이 종료되었습니다!");

        // TODO: 여기에 게임 오버 시 필요한 로직을 추가
        // 예: 스포너 작동 중지, 플레이어 조작 잠금, 결과창 UI 표시 등
    }
}