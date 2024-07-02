using System;
using UnityEngine;
using UnityEngine.AI;

namespace INeverFall.Monster
{
    /*
        A 패턴 : 플레이어가 있는 곳으로 돌진 (멀리 있을 때)
        B 패턴 : 플레이어 있는 부분 내려치기 (가까이 있을 때)
        // C 패턴 : 근처 오브젝트 들어서 던지기
        // D 패턴 : 탄막슈팅겜 패턴 (후순위)
     */
    [RequireComponent(typeof(CharacterController))]
    public class BossMonster : Monster
    {
        private Transform _eyesTransform;
        private Vector3 _spawnPosition;

        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        
        private PlayerCharacter _player;
        private BossStateMachine _stateMachine;
        
        private void Start()
        {
            // Caching
            _eyesTransform = transform.FindChildRecursively("Eyes");
            _spawnPosition = transform.position;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _player = FindFirstObjectByType<PlayerCharacter>();
            
            // State machine
            _stateMachine = new BossStateMachine(this);
            _stateMachine.Initialize(_stateMachine.IdleState);
            
            // Locomotion setting
            _animator.applyRootMotion = true;
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;
        }

        private void Update()
        {
            _stateMachine?.Update();
        }

        private void OnDrawGizmos()
        {
            if (_navMeshAgent != null)
            {
                if (_navMeshAgent.hasPath)
                {
                    for (var i = 0; i < _navMeshAgent.path.corners.Length - 1; i++)
                    {
                        var path = _navMeshAgent.path;
                        Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
                    }
                }
            }
        }

        private void OnAnimatorMove()
        {
            Vector3 rootPosition = _animator.rootPosition;
            rootPosition.y = _navMeshAgent.nextPosition.y;
            transform.position = rootPosition;
            _navMeshAgent.nextPosition = rootPosition;
            
            // _navMeshAgent.updateRotation = false일때,a
            //transform.rotation = _animator.rootRotation;
        }

        public PlayerCharacter Target => _player;
        public Animator Animator => _animator;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public BossStateMachine StateMachine => _stateMachine;
    }
}
