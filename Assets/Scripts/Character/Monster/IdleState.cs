using System.Collections;
using System.Collections.Generic;
using INeverFall;
using Unity.VisualScripting;
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

        // if문 없애고 깔끔하게 상태 전환 할 수 있는 방법 찾아보기
        public override void Update()
        {
            if (_IsTargetInAttackRange())
            {
                _stateMachine.TransitionTo(_stateMachine.FrontAttackState);
            }
            else
            {
                int random = Random.Range(0, 3);
                // 원거리 공격
                if (random == 0 && _stateMachine.DashAttackState.IsReady)
                {
                    _stateMachine.TransitionTo(_stateMachine.DashAttackState);
                    return;
                }
                else if (random == 1)
                {
                    _stateMachine.TransitionTo(_stateMachine.ThrowStoneAttackState);
                    return;
                }
                // 쫓아가기
                _stateMachine.TransitionTo(_stateMachine.TraceState);
            }
        }

        public override void Exit()
        {
            
        }

        private bool _IsTargetInAttackRange()
        {
            var bossPosition = _controller.transform.position;
            var playerPosition = _controller.Target.transform.position;

            return Vector3.Distance(bossPosition, playerPosition) < _controller.AttackDistance;
        }
    }
}