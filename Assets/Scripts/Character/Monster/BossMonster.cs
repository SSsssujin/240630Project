using System;
using System.Collections;
using INeverFall.Manager;
using INeverFall.Player;
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
            ThrowStone : 주변 오브젝트를 플레이어에게 던짐 
        가까이 있을 때
            Front : 랜덤 전방 공격 모션 재생
        n초에 한번씩
            // Bullet hell : 탄막슈팅겜 패턴 (후순위)
        
        [보충]
        1. 공격 모션 할 때, 보스 방향 플레이어 쪽으로 돌리기
        2. 도는 중에도 애니메이션 재생하기
        3. Dash 중간에 애니메이션 안 끊김
     */
    
    
    // Boss Skill Effect Controller
    // 
    
    [RequireComponent(typeof(CharacterController))]
    public partial class BossMonster : Monster
    {
        private Transform _eyesTransform;
        private Transform _rockTransform;
        
        private Vector3 _spawnPosition;
        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;

        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        
        private PlayerCharacter _player;
        private BossStateMachine _stateMachine;
        private BossStone _stone;
        private BossBodyEffect[] _bodyEffects;

        private int _damageCount;
        private const int _groggyEntryDamageCount = 5;

        protected override void _OnDamage()
        {
            // Notify to presenter
            var currentHpRate = (float)_hp / _maxHp;
            HealthChanged?.Invoke(currentHpRate);

            // if (_stateMachine.CurrentState != _stateMachine.GroggyState)
            // {
            //     _damageCount++;
            //     // [보충] Groggy 진입 조건 다시 생각해보기
            //     // n초 동안 m번 이상 공격 당했을때?
            //     if (_damageCount == _groggyEntryDamageCount)
            //     {
            //         _stateMachine.TransitionTo(_stateMachine.GroggyState);
            //         _damageCount = 0;
            //     }
            // }

            StartCoroutine(nameof(_cDamageColor));
        }
        
        private IEnumerator _cDamageColor()
        {
            var mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
            mat.color = Color.gray;
            yield return new WaitForSeconds(0.1f);
            mat.color = Color.white;
        }

        protected override void _OnDead()
        {
            _stateMachine.TransitionTo(_stateMachine.DeadState);
        }

        private void _AddAnimationEvents()
        {
            var animationClip = Utils.GetAnimationClipByType(_animator, BossAnimation.ThrowStone);
            animationClip.AddAnimationEvent(nameof(_StoneCreated), 1.25f);
            animationClip.AddAnimationEvent(nameof(_StoneThrown), 4.75f);
        }
        
        private void _StoneCreated()
        {
            _stone = ResourceManager.Instance.Instantiate("Rock", _rockTransform).DemandComponent<BossStone>();
            Vector3 thrownDirection = (_player.transform.position - transform.position).normalized;
            _stone.Create(this, thrownDirection, _player);
        }

        private void _StoneThrown()
        {
            _stone.MoveInDirection();
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
            bool shouldMove =_velocity.magnitude > 0.5f &&
                               _navMeshAgent.remainingDistance + distanceThreshold >
                               _navMeshAgent.stoppingDistance;
                              //|| _IsRotating();

            //_animator.SetBool(AnimationID.IsMoving, shouldMove);

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

        private bool _IsTargetWithinDistance(float distance)
        {
            var bossPosition = transform.position;
            var playerPosition = Target.transform.position;
            
            bool inDistance =  Vector3.Distance(bossPosition, playerPosition) < distance;
            return inDistance;
        }

        private bool _IsTargetWithinAttackAngle()
        {
            var bossPosition = transform.position;
            var playerPosition = Target.transform.position;
            
            var directionToPlayer = (playerPosition - bossPosition).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            bool inAngle = angleToPlayer < MaxAttackAngle;
            return inAngle;
        }

        public void ActivateEffect(BossAnimation effectType)
        {
            var desiredEffects = Array.FindAll(_bodyEffects, x => x.EffectType == effectType);
            foreach (var effect in desiredEffects)
            {
                effect.ActivateEffect();
            }
        }
        
        public void DeactivateEffect(BossAnimation effectType)
        {
            var desiredEffects = Array.FindAll(_bodyEffects, x => x.EffectType == effectType);
            foreach (var effect in desiredEffects)
            {
                effect.DeactivateEffect();
            }
        }

        public bool IsTargetWithinDistance(float a) => _IsTargetWithinDistance(a);
        public bool IsTargetWithinAttackAngle => _IsTargetWithinAttackAngle();

        public event System.Action<float> HealthChanged;

        public float AttackDistance { get; private set; } = 10;        
        public float MaxAttackAngle { get; private set; } = 45.0f;
        public Vector3? GroundAttackHandPosition { private get; set; }
        
        public bool IsTargetWithinAttackRange => _IsTargetWithinDistance(AttackDistance) && _IsTargetWithinAttackAngle();
        public PlayerCharacter Target => _player;
        public Animator Animator => _animator;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public BossStateMachine StateMachine => _stateMachine;
    }

    // Event functions
    public partial class BossMonster
    {
        protected override void Start()
        {
            // Caching
            _eyesTransform = transform.FindChildRecursively("Eyes");
            _rockTransform = transform.FindChildRecursively("Rock");
            _spawnPosition = transform.position;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _player = FindFirstObjectByType<PlayerCharacter>();
            _bodyEffects = GetComponentsInChildren<BossBodyEffect>(true);
            
            // State machine
            _stateMachine = new BossStateMachine(this);
            _stateMachine.Initialize(_stateMachine.IdleState);
            
            // Locomotion setting
            _animator.applyRootMotion = true;
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;
            
            // Additional objects
            _AddAnimationEvents();
            
            // Initialize variables
            _maxHp = _hp = 10000;
            _attackPower = 5;
        }

        private void Update()
        {
            _stateMachine?.Update();
            _SynchronizeNavMeshWithRootMotion();
        }

        private void OnAnimatorMove()
        {
            Vector3 rootPosition = _animator.rootPosition;
            rootPosition.y = _navMeshAgent.nextPosition.y;
            transform.position = rootPosition;
            _navMeshAgent.nextPosition = rootPosition;
        }
        
        private void OnAnimatorIK(int layerIndex)
        {
            if (!_animator) return;

            //_AdjustAttackingHandPosition();
        }
    }
}
