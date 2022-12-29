using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeAttackSkill : EffectOtherPlayerSkill
{
    [SerializeField] private float _damagePercentage = 1.3f;

    private void Awake()
    {
        Init();
    }

    protected override void EffectOtherPlayer (GameObject otherPlayer)
    {
        PlayerHealth _otherHealth = _playerMovement.OtherPlayer.GetComponent<PlayerHealth>();
        Debug.Assert(_otherHealth);

        _otherHealth.TakeDamage(Mathf.RoundToInt(_playerAct.Damage * _damagePercentage));
    }
}
