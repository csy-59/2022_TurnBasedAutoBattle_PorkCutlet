using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeAttackSkill : PlayerSkill
{
    // 플레이어
    [SerializeField] private Collider _playerCollider;
    [SerializeField] private Collider _stickCollider;
    [SerializeField] private PlayerMovement _playerMovement;

    [SerializeField] private Transform _targetPosition;
    private Vector3 _originalPosition;

    [SerializeField] private float _damagePercentage = 1.3f;

    private void Awake()
    {
        Init();
        _originalPosition = transform.position;
    }

    protected override void UseSkill()
    {
        Attack();
    }

    private void Attack()
    {
        PrepareForAttack();
        _playerMovement.MoveToPosition(_targetPosition.position, () =>
        {
            PlayerHealth _otherHealth = _playerMovement.OtherPlayer.GetComponent<PlayerHealth>();
            Debug.Assert(_otherHealth);

            _otherHealth.TakeDamage(Mathf.RoundToInt(_playerAct.Damage * _damagePercentage));

            _playerMovement.MoveToPosition(_originalPosition, () =>
            {
                _stickCollider.enabled = false;
                _playerCollider.enabled = true;
                OnUseSkill.Invoke(false);
            });
        });
    }

    private void PrepareForAttack()
    {
        _playerCollider.enabled = false;
        _stickCollider.enabled = true;
    }
}
