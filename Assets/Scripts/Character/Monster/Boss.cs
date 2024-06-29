using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

namespace INeverFall.Monster
{
    /*
        A 패턴 : 플레이어가 있는 곳으로 돌진 (멀리 있을 때)
        B 패턴 : 플레이어 있는 부분 내려치기 (가까이 있을 때)
        // C 패턴 : 근처 오브젝트 들어서 던지기
        // D 패턴 : 탄막슈팅겜 패턴 (후순위)
     */
    
    public class Boss : Monster
    {
        private Vector3 _spawnPosition;
        
        private PlayerCharacter _player;
        private BossStateMachine _stateMachine;
        
        private void Start()
        {
            _spawnPosition = transform.position;
            _stateMachine = new BossStateMachine(this);
            _stateMachine.Initialize(_stateMachine.IdleState);
        }

        private void Update()
        {
            _stateMachine?.Update();
        }

        public BossStateMachine StateMachine => _stateMachine;
    }
}
