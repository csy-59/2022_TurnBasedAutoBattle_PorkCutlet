using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EffectOtherPlayerSkill : PlayerSkill
{
    // 플레이어
    [SerializeField] private Collider _playerCollider;
    [SerializeField] private Collider _stickCollider;
    [SerializeField] protected PlayerMovement _playerMovement;

    [SerializeField] private Transform _targetPosition;
    private Vector3 _originalPosition;

    protected override void Init()
    {
        base.Init();
        _originalPosition = transform.position;
    }

    protected override void UseSkill()
    {
        if(_playerMovement.IsMoving)
        {
            _playerMovement.OnMovingOver.RemoveListener(UseSkill);
            _playerMovement.OnMovingOver.AddListener(UseSkill);
            return;
        }

        _playerMovement.IsMoving = true;
        Attack();
    }

    private void Attack()
    {
        PrepareForAttack();
        _playerMovement.MoveToPosition(_targetPosition.position, () =>
        {
            MoveToTargetAction();
        });
    }

    private void PrepareForAttack()
    {
        _playerCollider.enabled = false;
        _stickCollider.enabled = true;
    }

    protected virtual void MoveToTargetAction()
    {
        EffectOtherPlayer(_playerMovement.OtherPlayer);

        _playerMovement.MoveToPosition(_originalPosition, () =>
        {
            ReturnPositionAction();
        });
    }

    protected virtual void ReturnPositionAction()
    {
        _stickCollider.enabled = false;
        _playerCollider.enabled = true;
        _playerMovement.IsMoving = false;
        OnUseSkill.Invoke(false);
    }

    protected abstract void EffectOtherPlayer (GameObject otherPlayer);
}
