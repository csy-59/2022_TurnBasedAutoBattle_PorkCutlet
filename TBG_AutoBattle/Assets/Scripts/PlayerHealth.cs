using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public delegate void HpEvent();
    public HpEvent OnDeath;

    [SerializeField] private Slider _hpSlider;
    [SerializeField] private float _maxHp;
    private float _hp;
    private float _currentHp
    {
        get => _hp;
        set
        {
            _hp = value;
            _hpSlider.value = value;
            if(_hp == 0)
            {
                OnDeath.Invoke();
            }
        }
    }

    [SerializeField] private float _reducedSpeed = 0.5f;

    private void Awake()
    {
        _hpSlider.maxValue = _maxHp;
        _currentHp = _maxHp;
    }

    public void TakeDamage(float damage)
    {
        float newHp = Mathf.Clamp(_currentHp - damage, 0, _maxHp);
        StartCoroutine(ETakedDamage(newHp));
    }

    private IEnumerator ETakedDamage(float newHp)
    {
        float elapsedTime = 0f;
        float prevHp = _currentHp;

        while (true)
        {
            elapsedTime += Time.deltaTime * _reducedSpeed;
            _currentHp = Mathf.Lerp(0, newHp, elapsedTime);

            if (Mathf.Abs(newHp - _currentHp) < 0.1f)
            {
                _currentHp = newHp;
                break;
            }

            yield return null;
        }
    }
}
