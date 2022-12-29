using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerSkill : MonoBehaviour
{
    public UnityEvent<bool> OnUseSkill { get; set; } = new UnityEvent<bool>();

    private PlayerAct _playerAct;
    private PlayerHealth _playerHealth;

    protected int _turn;

    private void Init()
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
