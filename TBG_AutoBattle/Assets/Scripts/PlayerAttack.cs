using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    // 행동 속도
    [SerializeField] private float _maxAttackSpeed;
    [SerializeField] private float _minAttackSpeed;
    private float _attackSpeed;

    private Slider _behaviourSlider;
    private float _maxSliderValue = 1f;

    private void Awake()
    {
        _behaviourSlider = GetComponent<Slider>();

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
        Debug.Log("Attack");
        _behaviourSlider.value = 0f;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
