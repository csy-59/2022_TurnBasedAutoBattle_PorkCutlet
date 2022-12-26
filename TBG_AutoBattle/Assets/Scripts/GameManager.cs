using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerAttack[] _playerAttacks;
    private PlayerHealth[] _playerHealths;
    private bool _isGameOver;
    
    private void Awake()
    {
        _playerAttacks = GetComponentsInChildren<PlayerAttack>();
        _playerHealths = GetComponentsInChildren<PlayerHealth>();

        PlayerSetting();
    }

    private void PlayerSetting()
    {
        for(int i = 0;i<_playerAttacks.Length; ++i)
        {
            _playerAttacks[i].PlayerNumber = i;
            _playerAttacks[i].OnAttack -= OnAttack;
            _playerAttacks[i].OnAttack += OnAttack;

            _playerHealths[i].OnDeath -= OnPlayerDeath;
            _playerHealths[i].OnDeath += OnPlayerDeath;
        }
    }

    private void OnAttack(bool isStart, int playerNumber)
    {
        int otherNumber = playerNumber == 0 ? 1 : 0;

        if(isStart)
        {
            _playerAttacks[otherNumber].StopAttackCool();
        }
        else
        {
            if (_isGameOver)
            {
                foreach(PlayerAttack _playerAttack in _playerAttacks)
                {
                    _playerAttack.enabled = false;
                }
            }
            else
            {
                _playerAttacks[otherNumber].RestartAttackCool();
            }
        }
    }

    private void OnPlayerDeath()
    {
        _isGameOver = true;
    }
}
