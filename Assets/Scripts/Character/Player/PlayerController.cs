using System;
using DG.Tweening;
using INeverFall.Manager;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace INeverFall.Player
{
    [RequireComponent(typeof(Animator), typeof(CharacterController), typeof(PlayerInputs))]
    public class PlayerController : MonoBehaviour
    {
        private enum AttackState { None, First, Second, Last }

        private const float _threshold = 0.01f;
        private const float _terminalVelocity = 53.0f;
        private const int _attackAnimationSpeed = 2;

        [Header("Move")]
        public float MoveSpeed;
        public float SprintSpeed;
        public float SpeedChangeRate = 10.0f;
        public float RotationSmoothTime = 0.12f;
        public LayerMask GroundLayers;
        
        [Space(10)]

        [Header("Jump")]
        public float JumpTimeout = 0.50f;
        public float FallTimeout = 0.15f;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.28f;
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;

        [Space(10)]
        
        [Header("Attack")]
        public float _lastAttackTimeBet = 0.5f;

        // Move
        private bool _isMoving;
        private float _speed;
        private float _targetRotation;
        private float _rotationVelocity;
        private float _verticalVelocity;
        
        // Jump
        public bool _isGrounded;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        
        // Attack
        private bool _isAttacking;
        private float _attackTime;
        private float _animationSkipRate = 0.8f;
        private float _lastAttackTime;
        private float _attackDuration = 0.8f;
        private AttackState _attackState;

        private Animator _animator;
        private CharacterController _characterController;
        private PlayerCharacter _playerCharacter;
        
        // Inputs
        private PlayerInputs _playerInput;

        private void Start()
        {
            // Caching
            _playerInput = GetComponent<PlayerInputs>();
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _playerCharacter = GetComponent<CharacterBase>() as PlayerCharacter;
            
            // Initialize
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            _AddAnimationEvents();
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.IsGamePlaying) return;

            // Player Random Pos
            if (_playerInput.Jump)
            {
                transform.position = Vector3.zero;
            }
            

            _Jump();
            _CheckGround();

            if (!_playerCharacter.IsInvincible)
            {
                _MovePlayer();
                _Attack();
            }
        }

        private void _Jump()
        {
            if (_isGrounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // // Jump
                // if (_playerInput.Jump && _jumpTimeoutDelta <= 0.0f)
                // {
                //     // _animator.SetTrigger(AnimationID.Jump);
                //     //
                //     // // the square root of H * -2 * G = how much velocity needed to reach desired height
                //     // _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                // }
                
                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
            }
            
            // apply gravity over time
            // if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private Vector3 _playerPosition;
        
        private void _CheckGround()
        {
            _playerPosition.Set(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            _isGrounded = Physics.CheckSphere(_playerPosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
            _animator.SetBool(AnimationID.IsGrounded, _isGrounded);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_playerPosition, GroundedRadius);
        }

        public float SpeedOffset;
        public float Groundddddd;
        
        private void _MovePlayer()
        {
            if (_isAttacking)
            {
                _animator.SetFloat(AnimationID.VelocityX,0);
                _animator.SetFloat(AnimationID.VelocityZ,0);
                return;
            }
            
            // if (_playerInput.CameraLocked)
            // {
            //     //var camera = Object.FindFirstObjectByType<CameraSettings>().CurrentCamera;
            //     //camera.GetComponentInChildren<CinemachineRotationComposer>().enabled = false;
            // }
            // else
            {

                float targetSpeed = _playerInput.Sprint ? SprintSpeed : MoveSpeed;
                
                if (_playerInput.Move == Vector2.zero) targetSpeed = 0.0f;

                var controllerVelocity = _characterController.velocity;
                float currentHorizontalSpeed = new Vector3(controllerVelocity.x, 0.0f, controllerVelocity.z).magnitude;
                float speedOffset = SpeedOffset;
                float inputMagnitude = _playerInput.Move.magnitude;

                if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                    currentHorizontalSpeed > targetSpeed + speedOffset)
                {
                    _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                    _speed = Mathf.Round(_speed * 1000f) / 1000f;
                }
                else
                {
                    _speed = targetSpeed;
                }

                Vector3 inputDirection = new Vector3(_playerInput.Move.x, 0.0f, _playerInput.Move.y).normalized;

                if (_playerInput.Move != Vector2.zero)
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                      Camera.main.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation,
                        ref _rotationVelocity, RotationSmoothTime);

                    // rotate to face input direction relative to camera position
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }

                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

                _isMoving = _playerInput.Move != Vector2.zero;
                    
                if (_isMoving)
                {
                    // Adjust the input direction based on the character's rotation
                    var direction = transform.InverseTransformDirection(targetDirection);

                    _animator.SetFloat(AnimationID.VelocityX, direction.x * _speed);
                    _animator.SetFloat(AnimationID.VelocityZ, direction.z * _speed);
                    
                }
                else
                {
                    _animator.SetFloat(AnimationID.VelocityX,0);
                    _animator.SetFloat(AnimationID.VelocityZ,0);
                }
                
                _animator.SetBool(AnimationID.IsMoving, _isMoving);

                Vector3 movement = targetDirection.normalized *
                                   (_speed * Time.deltaTime) +
                                   new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime;
                
                // if (_isGrounded)
                // {
                     Ray ray = new Ray(_playerPosition, -Vector3.up);
                     if (Physics.Raycast(ray, out RaycastHit hit, GroundedRadius, GroundLayers,
                             QueryTriggerInteraction.Ignore))
                     {
                         // Get the movement of the player root rotated to lie along the plane of the ground.
                         movement = Vector3.ProjectOnPlane(movement, hit.normal);
                     }
                // }
                
                // move the player
                _characterController.Move(movement);
            }
        }

        private void _Attack()
        {
            if (!_isGrounded) return;
            
            // Do Attack
            if (_playerInput.Attack && !_isAttacking)
            {
                _isAttacking = true;

                // Reset attack state
                if (Time.time >= _lastAttackTime + _lastAttackTimeBet)
                {
                    _attackState = AttackState.None;
                }

                _attackState = _attackState switch
                {
                    AttackState.None => AttackState.First,
                    AttackState.First => AttackState.Second,
                    AttackState.Second => AttackState.Last,
                    AttackState.Last => AttackState.First,
                    _ => AttackState.None
                };
                
                _animationSkipRate = _attackState == AttackState.Last ? 0.6f : 0.5f;
                
                // Animation
                _animator.SetTrigger(AnimationID.Attack);
                _animator.SetInteger(AnimationID.AttackID, (int)_attackState);
                
                var currentClip = _animator.GetAnimationClipByName("Attack" + (int)_attackState);
                _attackDuration = (currentClip.length / _attackAnimationSpeed) * _animationSkipRate;
                
                //_DoAttack(_attackState);
                _lastAttackTime = Time.time + _attackDuration;
            }

            // Attack timer
            if (_isAttacking)
            {
                _attackTime += Time.deltaTime;
                
                if (_attackTime >= _attackDuration)
                {
                    _attackTime = 0;
                    _isAttacking = false;
                    _animator.SetInteger(AnimationID.AttackID, 0);
                }
            }
        }

        private void _AddAnimationEvents()
        {
            // Attack Effect
            // 1 Left
            // 2 Left Right
            // 3 Left Crash

            var firstAttackAnimationClip = _animator.GetAnimationClipByName("Attack1");
            firstAttackAnimationClip.AddAnimationEvent(nameof(_InstantiateEffect), 0.7f, 1); // left
            
            var secondAttackAnimationClip = _animator.GetAnimationClipByName("Attack2");
            secondAttackAnimationClip.AddAnimationEvent(nameof(_InstantiateEffect), 1f, 2); // left
            secondAttackAnimationClip.AddAnimationEvent(nameof(_InstantiateEffect), 2f, 3); // right 2

            var lastAttackAnimationClip = _animator.GetAnimationClipByName("Attack3");
            lastAttackAnimationClip.AddAnimationEvent(nameof(_InstantiateEffect), 1f, 4); // left
            lastAttackAnimationClip.AddAnimationEvent(nameof(_InstantiateEffect), 2.6f, 5); // crack 3
        }

        private void _InstantiateEffect(int skillId)
        {
            int skillNum = skillId switch
            {
                1 or 2 or 4 => 1,
                3 => 2,
                5 => 3,
                _ => 0
            };
            
            Transform effectTr = transform.Find("EffectTransform");
            
            GameObject fx = ResourceManager.Instance.Instantiate("Skill" + skillNum);
            fx.GetComponent<Skill>().Initialize(_playerCharacter, _playerCharacter.AttackPower);
            fx.transform.position = effectTr.GetChild(skillId - 1).position;
            fx.transform.rotation = effectTr.GetChild(skillId - 1).rotation;
        }   
    }
}