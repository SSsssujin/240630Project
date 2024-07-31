using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall.Player;
using INeverFall.Util;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool _isGameOver;

    private PlayerCharacter _playerCharacter;

    protected override void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        _isGameOver = false;
        
        // Caching
        _playerCharacter = FindFirstObjectByType<PlayerCharacter>();

        _playerCharacter.PlayerDead += _OnPlayerDead;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.visible == false ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

    private void _OnPlayerDead()
    {
        _isGameOver = true;
    }

    public bool IsGamePlaying => !_isGameOver;
}
