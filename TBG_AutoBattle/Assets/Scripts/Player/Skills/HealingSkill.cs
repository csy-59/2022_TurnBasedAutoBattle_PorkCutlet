using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingSkill : PlayerSkill
{
    [SerializeField] private float _healPersentage = 0.2f;
    
    private void Awake()
    {
        Init();
    }

    protected override void UseSkill()
    {
        _playerHealth.RestoreHp(Mathf.RoundToInt(_playerHealth.MaxHp * _healPersentage),
            () => { OnUseSkill.Invoke(false); });
    }
}
