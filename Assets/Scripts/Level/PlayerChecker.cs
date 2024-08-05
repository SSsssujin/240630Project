using System;
using INeverFall.Util;
using UnityEngine;

public class PlayerChecker : Singleton<PlayerChecker>
{
    private const string _playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayerInBossRoom) return;
        
        if (other.CompareTag(_playerTag))
        {
            IsPlayerInBossRoom = true;
            PlayerEntered?.Invoke();
        }
    }
    
    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag(_playerTag))
    //     {
    //         _isPlayerInBossRoom = false;
    //         PlayerExited?.Invoke();
    //     }
    // }
    
    public bool IsPlayerInBossRoom;

    public Action PlayerEntered;
    public Action PlayerExited;
}
