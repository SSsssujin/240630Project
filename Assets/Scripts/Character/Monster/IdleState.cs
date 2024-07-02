using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

namespace INeverFall.Monster
{
    public class IdleState : State
    {
        private BossMonster _controller;
        private PlayerCharacter _player;

        private BossStateMachine _stateMachine;

        public IdleState(BossMonster controller)
        {
            _controller = controller;
        }
        
        public override void Enter()
        {
            _stateMachine = _controller.StateMachine;
        }

        public override void Update()
        {
            if (_controller.Target is not null)
            {
                _stateMachine.TransitionTo(_stateMachine.TraceState);
            }
        }

        public override void Exit()
        {
            
        }
    }
}