using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAttackSkill : EffectOtherPlayerSkill
{
    [SerializeField] private float _damagePercentage = 0.9f;
    [SerializeField] private float _damageOffsetTime = 0.5f;
    private WaitForSeconds _waitForSecondDamage;

    private PlayerHealth _otherHealth;

    private void Awake()
    {
        Init();
        _waitForSecondDamage = new WaitForSeconds(_damageOffsetTime);
    }

    protected override void EffectOtherPlayer(GameObject otherPlayer)
    {
        _otherHealth = _playerMovement.OtherPlayer.GetComponent<PlayerHealth>();
        Debug.Assert(_otherHealth);

        _otherHealth.TakeDamage(Mathf.RoundToInt(_playerHealth.Defence * _damagePercentage));
        StartCoroutine(CoGiveDamage());
    }

    private IEnumerator CoGiveDamage()
    {
        yield return _waitForSecondDamage;
        _otherHealth.TakeDamage(Mathf.RoundToInt(_playerHealth.Defence * _damagePercentage));
    }
}
