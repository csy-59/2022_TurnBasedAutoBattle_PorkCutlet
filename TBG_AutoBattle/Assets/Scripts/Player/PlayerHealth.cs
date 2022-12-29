using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public UnityEvent OnDeath { get; set; } = new UnityEvent();

    [Header("=== Basic State ===")]
    [SerializeField] private int _maxHp;
    public int MaxHp { get => _maxHp; set => _maxHp = value; }
    [SerializeField] private int _defence;
    public int Defence { get => _defence; set => _defence = value; }
    private float _hp;
    private float _currentHp
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

    [Header("=== UI & Effect ===")]
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private float _uiChangeSpeed = 0.5f;
    [SerializeField] private HPTextEffect _textEffect;

    private void Awake()
    {
        _hpSlider.maxValue = _maxHp;
        _currentHp = _maxHp;
    }

    public void TakeDamage(float damage)
    {
        float newHp = Mathf.Clamp(_currentHp + _defence - damage, 0, _maxHp);
        _textEffect.ShowEffect($"-{damage}");
        StartCoroutine(CoChangeHp(newHp));
    }

    public void RestoreHp(float restoreHp, UnityAction onHpRestore)
    {
        float newHp = Mathf.Clamp(_currentHp + restoreHp, 0, _maxHp);
        _textEffect.ShowEffect($"+{restoreHp}");
        StartCoroutine(CoChangeHp(newHp, onHpRestore));
    }

    private IEnumerator CoChangeHp(float newHp, UnityAction onHpRestore = null)
    {
        float prevHp = _currentHp;

        while (true)
        {
            _currentHp = Mathf.Lerp(_currentHp, newHp, _uiChangeSpeed * Time.deltaTime);

            if (Mathf.Abs(newHp - _currentHp) < 0.1f)
            {
                _currentHp = newHp;
                break;
            }

            yield return null;
        }

        onHpRestore?.Invoke();
    }
}
