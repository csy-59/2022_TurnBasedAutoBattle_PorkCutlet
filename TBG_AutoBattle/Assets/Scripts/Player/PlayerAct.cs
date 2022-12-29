using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerAct : MonoBehaviour
{
    public UnityEvent<bool, int> OnBehaviour { get; set; } = new UnityEvent<bool, int>();
    public int PlayerNumber { get; set; }

    public UnityEvent OnAct { get; set; } = new UnityEvent();

    // 턴 속도
    [SerializeField] private float _maxActCoolSpeed;
    [SerializeField] private float _minActCoolSpeed;
    private float _actCoolTime;
    private IEnumerator _actCoolCoroutine;

    // 기본 데미지
    [SerializeField] private int _damage;
    public int Damage { get => _damage; set => _damage = value; }

    // UI: 슬라이더
    [SerializeField] private Slider _behaviourSlider;
    private float _maxSliderValue = 1f;

    [SerializeField] private PlayerSkill[] _skills;


    // 플레이어
    [SerializeField] private Collider _stickCollider;
    private Collider _playerCollider;
    [SerializeField] private Transform _targetPosition;
    private Vector3 _originalPosition;
    [SerializeField] private float _moveSpeed;

    private void Awake()
    {
        SetRandomSpeed();

        ConnectWithSkill();

        _actCoolCoroutine = CoAttackCool();
        StartCoroutine(_actCoolCoroutine);
    }

    private void ConnectWithSkill()
    {
        foreach (PlayerSkill skill in _skills)
        {
            skill.OnUseSkill.RemoveListener(ActBahaviour);
            skill.OnUseSkill.AddListener(ActBahaviour);
        }
    }

    private void ActBahaviour(bool value)
    {
        if(value)
        {
            OnBehaviour.Invoke(true, PlayerNumber);
        }
        else
        {
            _actCoolCoroutine = CoAttackCool();
            StartCoroutine(_actCoolCoroutine);
            OnBehaviour.Invoke(false, PlayerNumber);
        }
    }

    private void SetRandomSpeed()
    {
        _actCoolTime = Random.Range(_minActCoolSpeed, _maxActCoolSpeed);
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

        OnAct.Invoke();
        //Attack();
    }

    public void StopAttackCool()
    {
        StopCoroutine(_actCoolCoroutine);
    }

    public void RestartAttackCool()
    {
        StartCoroutine(_actCoolCoroutine);
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
                        _actCoolCoroutine = CoAttackCool();
                        StartCoroutine(_actCoolCoroutine);
                        OnBehaviour.Invoke(false, PlayerNumber);
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
        OnBehaviour.Invoke(true, PlayerNumber);
    }

    private IEnumerator CoMoveToPosition(Vector3 targetPosition, UnityAction afterMoveAction)
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
