using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerAct : MonoBehaviour
{
    public UnityEvent<bool, int> OnAttack { get; set; } = new UnityEvent<bool, int>();

    public UnityEvent OnAct { get; set; } = new UnityEvent();

    public int PlayerNumber { get; set; }

    private delegate void MoveAction();

    // 턴 속도
    [SerializeField] private float _maxActCoolSpeedㅊ;
    [SerializeField] private float _minActCoolSpeed;
    private float _actCoolTime;


    [SerializeField] private float _moveSpeed;
    private IEnumerator _attackCoolCoroutine;

    // 기본 데미지
    [SerializeField] private Transform _targetPosition;
    private Vector3 _originalPosition;
    [SerializeField] private float _damage;

    // 플레이어
    [SerializeField] private Collider _stickCollider;
    private Collider _playerCollider;

    // UI: 슬라이더
    [SerializeField] private Slider _behaviourSlider;
    private float _maxSliderValue = 1f;

    private void Awake()
    {
        _playerCollider = GetComponent<Collider>();

        _originalPosition = transform.position;
        
        SetRandomSpeed();

        _attackCoolCoroutine = CoAttackCool();
        StartCoroutine(_attackCoolCoroutine);
    }

    private void SetRandomSpeed()
    {
        _actCoolTime = Random.Range(_minActCoolSpeed, _maxActCoolSpeedㅊ);
    }

    #region AttackCoolCoroutine

    private IEnumerator CoAttackCool()
    {
        _behaviourSlider.value = 0f;
        float elapsedTime = 0f;
        float currentValue = 0f;

        while(true)
        {
            elapsedTime += Time.deltaTime * _actCoolTime;
            currentValue = Mathf.Lerp(0, _maxSliderValue, elapsedTime);
            _behaviourSlider.value = currentValue;

            if (Mathf.Abs(_maxSliderValue - currentValue) < 0.01f)
            {
                _behaviourSlider.value = _maxSliderValue;
                break;
            }

            yield return null;
        }

        Attack();
    }

    public void StopAttackCool()
    {
        StopCoroutine(_attackCoolCoroutine);
    }

    public void RestartAttackCool()
    {
        StartCoroutine(_attackCoolCoroutine);
    }
    #endregion

    private void Attack()
    {
        _behaviourSlider.value = 0f;
        PrepareForAttack();
        StartCoroutine(CoMoveToPosition(_targetPosition.position, 
            () =>
            {
                StartCoroutine(CoMoveToPosition(_originalPosition,
                    () =>
                    {
                        _attackCoolCoroutine = CoAttackCool();
                        StartCoroutine(_attackCoolCoroutine);
                        OnAttack.Invoke(false, PlayerNumber);
                        _stickCollider.enabled = false;
                        _playerCollider.enabled = true;
                    }
                    ));
            }
            ));
    }

    private void PrepareForAttack()
    {
        _playerCollider.enabled = false;
        _stickCollider.enabled = true;
        OnAttack.Invoke(true, PlayerNumber);
    }

    private IEnumerator CoMoveToPosition(Vector3 targetPosition, MoveAction afterMoveAction)
    {
        while (true)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.deltaTime);

            transform.position = newPosition;

            if ((transform.position - targetPosition).sqrMagnitude < 0.001)
            {
                transform.position = targetPosition;
                break;
            }

            yield return null;
        }

        afterMoveAction.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth _otherHealth = other.GetComponent<PlayerHealth>();
            Debug.Assert(_otherHealth);

            _otherHealth.TakeDamage(_damage);
        }
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
