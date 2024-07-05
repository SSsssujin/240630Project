using UnityEngine;
using UnityEngine.AI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace INeverFall.Monster
{
    /*
        [패턴]
        // 처음 진입 시 atk_jump 해도 괜찮을듯?
        기본
            Idle
        멀리 있을 때
            Trace : 플레이어 쪽으로 걸어서 쫓아감
            DashAttack : 플레이어가 있는 곳으로 돌진
            // ThrowStone : 주변 오브젝트를 플레이어에게 던짐 
        가까이 있을 때
            Front : 랜덤 전방 공격 모션 재생
        n초에 한번씩
            // Bullet hell : 탄막슈팅겜 패턴 (후순위)
        
        [보충]
        1. 공격 모션 할 때, 보스 방향 플레이어 쪽으로 돌리기
        2. 도는 중에도 애니메이션 재생하기
     */
    [RequireComponent(typeof(CharacterController))]
    public class BossMonster : Monster
    {
        private Transform _eyesTransform;
        private Vector3 _spawnPosition;
        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;

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
            _SynchronizeNavMeshWithRootMotion();
        }

        private void _SynchronizeNavMeshWithRootMotion()
        {
            Vector3 worldDeltaPosition = _navMeshAgent.nextPosition - transform.position;
            worldDeltaPosition.y = 0;

            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);

            float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
            _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);

            _velocity = _smoothDeltaPosition / Time.deltaTime;
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                _velocity = Vector2.Lerp(
                    Vector2.zero,
                    _velocity,
                    _navMeshAgent.remainingDistance / _navMeshAgent.stoppingDistance);
            }

            float distanceThreshold = 1f;
            bool shouldMove = _velocity.magnitude > 0.5f &&
                              _navMeshAgent.remainingDistance + distanceThreshold >
                              _navMeshAgent.stoppingDistance;

            _animator.SetBool(AnimationID.IsMoving, shouldMove);

            // To avoid obstacle invasion
            float deltaMagnitude = worldDeltaPosition.magnitude;
            if (deltaMagnitude > _navMeshAgent.radius / 2f)
            {
                transform.position = Vector3.Lerp(
                    _animator.rootPosition,
                    _navMeshAgent.nextPosition,
                    smooth);
            }
        }

        private void OnAnimatorMove()
        {
            Vector3 rootPosition = _animator.rootPosition;
            rootPosition.y = _navMeshAgent.nextPosition.y;
            transform.position = rootPosition;
            _navMeshAgent.nextPosition = rootPosition;

            // [보충]
            // 1. 공격 모션 할 때, 보스 방향 플레이어 쪽으로 돌리기
            // 2. 도는 중에도 애니메이션 재생하기
            // _navMeshAgent.updateRotation = false일때,
            //transform.rotation = _animator.rootRotation;
        }
        
        private void OnAnimatorIK(int layerIndex)
        {
            if (!_animator) return;

            //_AdjustAttackingHandPosition();
        }

        // [보충]
        // Penetrating the ground collider 하는 것을 방지하기 위함
        private void _AdjustAttackingHandPosition()
        {
            // When attack ground
            if (_animator.IsSpecificAnimationPlaying(BossAnimation.GroundAttack))
            {
                Vector3 currentHandPosition = _animator.GetIKPosition(AvatarIKGoal.RightHand);
                float groundPositionY = currentHandPosition.y;
                float rayDistance = 5f;

                if (GroundAttackHandPosition != null)
                {
                    groundPositionY = GroundAttackHandPosition.Value.y;
                }
                
                // Adjust the hand position based on the ground height
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                Vector3 targetPosition = _animator.GetIKPosition(AvatarIKGoal.RightHand);
                targetPosition.y = Mathf.Max(targetPosition.y, groundPositionY);
                _animator.SetIKPosition(AvatarIKGoal.RightHand, targetPosition);
            }
        }

        private void _ResetSaveHandPosition()
        {
            GroundAttackHandPosition = null;
        }
        
        public Vector3? GroundAttackHandPosition { private get; set; }
        public float AttackDistance { get; private set; } = 10;
        public PlayerCharacter Target => _player;
        public Animator Animator => _animator;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public BossStateMachine StateMachine => _stateMachine;
    }
}
