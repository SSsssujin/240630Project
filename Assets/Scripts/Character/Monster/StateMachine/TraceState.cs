using System.Collections;
using System.Collections.Generic;
using INeverFall;
using Unity.Cinemachine;
using UnityEngine;

namespace INeverFall.Monster
{
    public class TraceState : IState
    {
        private readonly BossMonster _controller;
        
        public TraceState(BossMonster controller)
        {
            _controller = controller;
        }

        private GameObject go;

        public void Enter()
        {
            _controller.Animator.SetBool(AnimationID.IsMoving, true);
        }

        public void Update()
        {
            _UpdatePosition();
            
            if (_controller.StateMachine.FrontAttackState.IsReady)
            {
                _controller.StateMachine.TransitionTo(_controller.StateMachine.FrontAttackState);
            }
            else if (_controller.StateMachine.DashAttackState.IsReady)
            {
                _controller.StateMachine.TransitionTo(_controller.StateMachine.DashAttackState);
            }
            else if (_controller.StateMachine.ThrowStoneAttackState.IsReady)
            {
                _controller.StateMachine.TransitionTo(_controller.StateMachine.ThrowStoneAttackState);
            }
        }

        public void Exit()
        {
            _controller.Animator.SetBool(AnimationID.IsMoving, false);
        }

        private void _UpdatePosition()
        {
            _controller.NavMeshAgent.SetDestination(_controller.Target.transform.position);
        }
    }
}