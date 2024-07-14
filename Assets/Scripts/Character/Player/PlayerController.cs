using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace INeverFall.Player
{
    [RequireComponent(typeof(Animator), typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput), typeof(PlayerInputs))]
    public class PlayerController : MonoBehaviour
    {
        private const float _threshold = 0.01f;
        private const float _terminalVelocity = 53.0f;

        private float _attackDuration = 0.8f;


        [Header("Move")]
        public float MoveSpeed;
        public float SprintSpeed;
        public float SpeedChangeRate = 10.0f;
        public float RotationSmoothTime = 0.12f;
        
        [Space(10)]

        [Header("Jump")]
        public float JumpTimeout = 0.50f;
        public float FallTimeout = 0.15f;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.28f;
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;
        public LayerMask GroundLayers;

        // Move
        private float _speed;
        private float _targetRotation;
        private float _rotationVelocity;
        private float _verticalVelocity;
        
        // Jump
        private bool _isGrounded;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        
        // Attack
        private bool _isAttacking;
        private float _attackTime;

        // Cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        private Animator _animator;
        private Weapon _weapon;
        private CharacterController _characterController;
        private PlayerCharacter _playerCharacter;
        
        // Inputs
        private PlayerInput _playerInput;
        private PlayerInputs _keyInput;

        private void Start()
        {
            // Caching
            _keyInput = GetComponent<PlayerInputs>();
            _playerInput = GetComponent<PlayerInput>();
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _weapon = GetComponentInChildren<Weapon>();
            _playerCharacter = GetComponent<CharacterBase>() as PlayerCharacter;
            
            _cinemachineTargetYaw = transform.rotation.eulerAngles.y;
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            _Jump();
            _CheckGround();
            _MovePlayer();
            _Attack();
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

                // Jump
                if (_keyInput.Jump && _jumpTimeoutDelta <= 0.0f)
                {
                    _animator.SetTrigger(AnimationID.Jump);
                    
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                }
                
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
        
        private void _CheckGround()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            _isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
            
            _animator.SetBool(AnimationID.IsGrounded, _isGrounded);
        }

        private bool _isMoving;
        
        private void _MovePlayer()
        {
            if (_isMoveBlocked) return;
            
            if (_keyInput.CameraLocked)
            {
                //var camera = Object.FindFirstObjectByType<CameraSettings>().CurrentCamera;
                //camera.GetComponentInChildren<CinemachineRotationComposer>().enabled = false;
            }
            else
            {
                float targetSpeed = _keyInput.Sprint ? SprintSpeed : MoveSpeed;
                
                if (_keyInput.Move == Vector2.zero) targetSpeed = 0.0f;

                var controllerVelocity = _characterController.velocity;
                float currentHorizontalSpeed = new Vector3(controllerVelocity.x, 0.0f, controllerVelocity.z).magnitude;
                float speedOffset = 0.1f;
                float inputMagnitude = _keyInput.Move.magnitude;

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

                Vector3 inputDirection = new Vector3(_keyInput.Move.x, 0.0f, _keyInput.Move.y).normalized;

                if (_keyInput.Move != Vector2.zero)
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                      Camera.main.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation,
                        ref _rotationVelocity, RotationSmoothTime);

                    // rotate to face input direction relative to camera position
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }

                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

                _isMoving = _keyInput.Move != Vector2.zero;
                    
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

                // move the player
                _characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }
        }

        private bool _isMoveBlocked;

        private void _Attack()
        {
            // Do Attack
            if (_keyInput.Attack && !_isAttacking)
            {
                _isAttacking = true;
                _animator.SetTrigger(AnimationID.Attack);
                _weapon.DoAttack(_playerCharacter, transform.forward);
                
                if (!_isMoving)
                {
                    // Select random attack animation
                    int randomId = Random.Range(2, 9); 
                    _animator.SetInteger(AnimationID.AttackID, randomId);
                    AnimationClip clip = Utils.GetAnimationClipByName(_animator, "2Hand-Sword-Attack" + randomId);
                    _attackDuration = clip.length;
                    
                }
                else
                {
                    _attackDuration = 0.8f;
                }
            }

            // Attack timer
            if (_isAttacking)
            {
                _attackTime += Time.deltaTime;
                _isMoveBlocked = true;
                
                if (_attackTime >= _attackDuration)
                {
                    _isAttacking = false;
                    _attackTime = 0;
                    _animator.ResetTrigger(AnimationID.Attack);
                    _isMoveBlocked = false;
                }
            }
        }
    }
}