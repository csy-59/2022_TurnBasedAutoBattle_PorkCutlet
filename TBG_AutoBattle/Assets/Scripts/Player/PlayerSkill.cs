using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerSkill : MonoBehaviour
{
    public UnityEvent<bool> OnUseSkill { get; set; } = new UnityEvent<bool>();

    protected PlayerAct _playerAct;
    protected PlayerHealth _playerHealth;

    [SerializeField] private int _maxTurn;
    protected int _turn;

    protected virtual void Init()
    {
        ResetTurn();
        OnUseSkill.AddListener(ResetTurn);

        _playerAct = GetComponent<PlayerAct>();
        _playerAct.OnAct.AddListener(() => 
        {
            _turn--; 
            if(_turn <= 0)
            {
                OnUseSkill.Invoke(true);
                UseSkill();
            }
        });

        _playerHealth = GetComponent<PlayerHealth>();
    }

    private void ResetTurn()
    {
        _turn = _maxTurn;
    }
    private void ResetTurn(bool value)
    {
        if(value)
        {
            ResetTurn();
        }
    }

    protected abstract void UseSkill();
}
