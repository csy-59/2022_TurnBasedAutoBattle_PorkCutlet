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

    // 스킬
    [SerializeField] private PlayerSkill[] _skills;
    private int _skillCount;

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
            if(_skillCount == 0)
            {
                OnBehaviour.Invoke(true, PlayerNumber);
            }
            _skillCount++;
        }
        else
        {
            _skillCount--;
            if(_skillCount == 0)
            {
                _actCoolCoroutine = CoAttackCool();
                StartCoroutine(_actCoolCoroutine);
                OnBehaviour.Invoke(false, PlayerNumber);
            }
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

        _behaviourSlider.value = 0f;
        OnAct.Invoke();
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

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
