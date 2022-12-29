using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAttackSkill : EffectOtherPlayerSkill
{
    [SerializeField] private int _basicAttack = 10;
    [SerializeField] private float _shieldAttackPercentage = 0.1f;

    private void Awake()
    {
        Init();
    }

    protected override void EffectOtherPlayer(GameObject otherPlayer)
    {
        PlayerHealth _otherHealth = _playerMovement.OtherPlayer.GetComponent<PlayerHealth>();
        Debug.Assert(_otherHealth);

        _otherHealth.TakeDamage(_basicAttack + Mathf.RoundToInt(_playerHealth.Defence * _shieldAttackPercentage));
    }
}
