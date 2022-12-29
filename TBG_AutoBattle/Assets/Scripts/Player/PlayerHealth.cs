using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public delegate void HpEvent();
    public HpEvent OnDeath;

    [SerializeField] private float _maxHp;
    [SerializeField] private float _defence;
    private float _hp;
    public float CurrentHp
    {
        get => _hp;
        set
        {
            _hp = value;
            _hpSlider.value = value;
            if (_hp == 0)
            {
                OnDeath.Invoke();
            }
        }
    }

    [SerializeField] private Slider _hpSlider;

    [SerializeField] private float _reducedSpeed = 0.5f;

    [SerializeField] private DamageTextEffect _textEffect;

    private void Awake()
    {
        _hpSlider.maxValue = _maxHp;
        CurrentHp = _maxHp;
    }

    public void TakeDamage(float damage)
    {
        float newHp = Mathf.Clamp(CurrentHp + _defence - damage, 0, _maxHp);
        _textEffect.ShowEffect(damage);
        StartCoroutine(CoTakedDamage(newHp));
    }

    private IEnumerator CoTakedDamage(float newHp)
    {
        float prevHp = CurrentHp;

        while (true)
        {
            CurrentHp = Mathf.Lerp(CurrentHp, newHp, _reducedSpeed * Time.deltaTime);

            if (Mathf.Abs(newHp - CurrentHp) < 0.1f)
            {
                CurrentHp = newHp;
                break;
            }

            yield return null;
        }
    }
}
