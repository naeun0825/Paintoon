// MagicCircleController.cs
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MagicCircleController : MonoBehaviour
{
    [Header("참조")]
    public GestureRecorder gestureRecorder;
    //public GestureRecognizer gestureRecognizer; // 템플릿 비교기 (다음에 만들 것)

    [Header("템플릿")]
    public GestureTemplate skill1Template;      // Circle 고정
    public List<GestureTemplate> skill2Templates; // 랜덤 풀

    [Header("입력")]
    public InputActionReference rightGripAction;
    public InputActionReference leftGripAction;
    public InputActionReference triggerAction;

    [Header("이펙트")]
    public TrailRenderer drawingTrail;
    public ParticleSystem projectileEffect;

    public MagicGuideDisplay magicGuideDisplay;
    public AccuracyDisplay accuracyDisplay;

    private bool _isDrawing = false;
    private GestureTemplate _currentTemplate; // 현재 선택된 템플릿
    private string _readyMagic = "";
    private float _magicAccuracy = 0f;

    private void Update()
    {
        float rightGrip = rightGripAction.action.ReadValue<float>();
        float leftGrip = leftGripAction.action.ReadValue<float>();
        float trigger = triggerAction.action.ReadValue<float>();

        // 오른손 Grip 누름
        if (rightGrip > 0.8f && !_isDrawing)
        {
            _isDrawing = true;
            _readyMagic = "";
            drawingTrail.gameObject.SetActive(true);

            // 스킬 1 or 2 결정
            if (leftGrip > 0.8f)
            {
                // 스킬 2: 랜덤 템플릿 선택
                int idx = Random.Range(0, skill2Templates.Count);
                _currentTemplate = skill2Templates[idx];
                Debug.Log($"스킬 2 - {_currentTemplate.magicName} 모양");
            }
            else
            {
                // 스킬 1: 고정 Circle
                _currentTemplate = skill1Template;
                Debug.Log("스킬 1 - Circle 고정");
            }

            gestureRecorder.StartRecording();
            magicGuideDisplay.ShowGuide(_currentTemplate);
        }

        // 오른손 Grip 궤적 기록
        if (_isDrawing && rightGrip > 0.8f)
        {
            gestureRecorder.AddPoint(transform.position);
        }

        // 오른손 Grip 인식
        if (_isDrawing && rightGrip <= 0.8f)
        {
            _isDrawing = false;
            magicGuideDisplay.HideGuide();
            drawingTrail.gameObject.SetActive(false);

            List<Vector3> raw = gestureRecorder.StopRecording();
            List<Vector3> projected = GestureNormalizer.ProjectToBestPlane(raw);
            List<Vector3> normalized = GestureNormalizer.Normalize(projected);

            // 새로운 특징 기반 정확도 계산 (다음에 만들 GestureAnalyzer 사용)
            float accuracy = GestureAnalyzer.Analyze(normalized, _currentTemplate);

            if (accuracy >= _currentTemplate.minAccuracy)
            {
                _readyMagic = _currentTemplate.magicName;
                _magicAccuracy = accuracy;
                Debug.Log($"마법 준비됨: {_readyMagic} (정확도: {accuracy:P0})");
            }
            else
            {
                Debug.Log($"마법진 인식 실패 (정확도: {accuracy:P0})");
            }
            accuracyDisplay.ShowAccuracy(accuracy);
        }

        // Trigger
        if (!string.IsNullOrEmpty(_readyMagic) && trigger > 0.8f)
        {
            FireMagic(_readyMagic, _magicAccuracy);
            _readyMagic = "";
        }
    }

    private void FireMagic(string magicName, float accuracy)
    {
        projectileEffect.transform.position = transform.position;
        projectileEffect.transform.rotation = transform.rotation;
        projectileEffect.Play();

        int damage = Mathf.RoundToInt(accuracy * 3f);
        damage = Mathf.Max(1, damage); // 최소 1 데미지

        // Enemy 레이어 마스크 설정
        int enemyLayer = LayerMask.GetMask("Enemy");

        // 오른손 컨트롤러 방향으로 Raycast 발사
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, enemyLayer))
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"{magicName} 적 명중! 데미지: {damage} (정확도: {accuracy:P0})");
            }
        }
        else
        {
            Debug.Log($"{magicName} 빗나감!");
        }
    }
}