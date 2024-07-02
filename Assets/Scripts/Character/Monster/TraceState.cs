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

        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;

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
            _Synchronize();

            if (_HasReachedDestination())
            {
                _controller.StateMachine.TransitionTo(_controller.StateMachine.AttackState);
            }
        }

        public override void Exit()
        {
        }

        private void _UpdatePosition()
        {
            _controller.NavMeshAgent.SetDestination(_controller.Target.transform.position);
        }

        private void _Synchronize()
        {
            var transform = _controller.transform;

            Vector3 worldDeltaPosition = _controller.NavMeshAgent.nextPosition - transform.position;
            worldDeltaPosition.y = 0;

            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(_controller.transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);

            float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
            _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);

            _velocity = _smoothDeltaPosition / Time.deltaTime;
            if (_controller.NavMeshAgent.remainingDistance <= _controller.NavMeshAgent.stoppingDistance)
            {
                _velocity = Vector2.Lerp(
                    Vector2.zero,
                    _velocity,
                    _controller.NavMeshAgent.remainingDistance / _controller.NavMeshAgent.stoppingDistance);
            }


            float distanceThreshold = 0.75f;
            bool shouldMove = _velocity.magnitude > 0.5f &&
                              _controller.NavMeshAgent.remainingDistance + distanceThreshold >
                              _controller.NavMeshAgent.stoppingDistance;

            _controller.Animator.SetBool(AnimationID.IsMoving, shouldMove);

            // To avoid obstacle invasion
            float deltaMagnitude = worldDeltaPosition.magnitude;
            if (deltaMagnitude > _controller.NavMeshAgent.radius / 2f)
            {
                _controller.transform.position = Vector3.Lerp(
                    _controller.Animator.rootPosition,
                    _controller.NavMeshAgent.nextPosition,
                    smooth);
            }
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