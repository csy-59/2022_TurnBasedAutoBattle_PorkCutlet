using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerSkill : MonoBehaviour
{
    public UnityEvent<bool> OnSkillUsed { get; set; } = new UnityEvent<bool>();

    protected PlayerAct _playerAct;
    protected PlayerHealth _playerHealth;

    protected int _turn;

    protected void Init()
    {
        _playerAct = GetComponent<PlayerAct>();
        _playerAct.OnAct.AddListener(() => 
        { 
            _turn--; 
            if(_turn <= 0)
            {
                UseSkill();
            }
        });

        _playerHealth = GetComponent<PlayerHealth>();
    }

    protected abstract void UseSkill();
}
