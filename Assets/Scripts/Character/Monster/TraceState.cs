using System.Collections;
using System.Collections.Generic;
using INeverFall;
using Unity.Cinemachine;
using UnityEngine;

namespace INeverFall.Monster
{
    public class TraceState : State
    {
        private readonly BossMonster _controller;

        public TraceState(BossMonster controller)
        {
            _controller = controller;
        }

        public override void Enter()
        {
            
        }

        public override void Update()
        {
            _UpdatePosition();

            if (_HasReachedDestination())
            {
                _controller.StateMachine.TransitionTo(_controller.StateMachine.IdleState);
            }
            else if (_controller.StateMachine.DashAttackState.IsReady)
            {
                _controller.StateMachine.TransitionTo(_controller.StateMachine.DashAttackState);
            }
        }

        public override void Exit()
        {
            _controller.Animator.SetBool(AnimationID.IsMoving, false);
        }

        private void _UpdatePosition()
        {
            _controller.NavMeshAgent.SetDestination(_controller.Target.transform.position);
        }

        private bool _HasReachedDestination()
        {
            var agent = _controller.NavMeshAgent;
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}