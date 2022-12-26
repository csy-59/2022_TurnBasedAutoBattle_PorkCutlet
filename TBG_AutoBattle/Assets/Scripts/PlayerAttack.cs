using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public delegate void AttackBehaviour();
    public AttackBehaviour OnAttack;

    // 행동 속도
    [SerializeField] private float _maxAttackSpeed;
    [SerializeField] private float _minAttackSpeed;
    private float _attackSpeed;

    // 기본 데미지
    [SerializeField] private float _damage;

    // 플레이어
    private Collider _playerCollider;

    // UI: 슬라이더
    [SerializeField] private Slider _behaviourSlider;
    private float _maxSliderValue = 1f;

    private void Awake()
    {
        _playerCollider = GetComponent<Collider>();
        
        SetRandomSpeed();

        StartCoroutine(EAttackCool());
    }

    private void SetRandomSpeed()
    {
        _attackSpeed = Random.Range(_minAttackSpeed, _maxAttackSpeed);
    }

    private IEnumerator EAttackCool()
    {
        _behaviourSlider.value = 0f;
        float elapsedTime = 0f;
        float currentValue = 0f;

        while(true)
        {
            elapsedTime += Time.deltaTime * _attackSpeed;
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


    private void Attack()
    {
        _behaviourSlider.value = 0f;
        PrepareForAttack();
    }

    private void PrepareForAttack()
    {
        _playerCollider.enabled = false;
        OnAttack.Invoke();
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
        _behaviourSlider.gameObject.SetActive(false);
    }
}
