using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace INeverFall.Player
{
    [RequireComponent(typeof(Animator), typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput), typeof(PlayerInputs))]
    public class PlayerController : MonoBehaviour
    {
        private const float _threshold = 0.01f;
        private const float _terminalVelocity = 53.0f;

        [Header("Move")]
        public float MoveSpeed;
        public float SprintSpeed;
        public float SpeedChangeRate = 10.0f;
        public float RotationSmoothTime = 0.12f;
        
        [Space(10)]

        [Header("Jump")]
        public bool Grounded = true;
        public float JumpTimeout = 0.50f;
        public float FallTimeout = 0.15f;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.28f;
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;
        public LayerMask GroundLayers;

        private float _speed;
        private float _targetRotation;
        private float _rotationVelocity;
        private float _verticalVelocity;
        
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // Cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        private CharacterController _characterController;
        private Animator _animator;
        
        // Inputs
        private PlayerInput _playerInput;
        private PlayerInputs _keyInput;

        private FirstPersonMovement _fpsMovement;
        private ThirdPersonMovement _tpsMovement;
        
        private void Start()
        {
            _keyInput = GetComponent<PlayerInputs>();
            _playerInput = GetComponent<PlayerInput>();
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
         
            _cinemachineTargetYaw = transform.rotation.eulerAngles.y;
            
            _fpsMovement = new();
            _tpsMovement = new();
            
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            _Jump();
            _CheckGround();
            _MovePlayer();
        }

        private void _Jump()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_keyInput.Jump && _jumpTimeoutDelta <= 0.0f)
                {
                    Debug.Log("Jump");
                    
                    _animator.SetTrigger("Jump");
                    
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    // if (_hasAnimator)
                    // {
                    //     _animator.SetBool(_animIDJump, true);
                    // }
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
                else
                {
                    // update animator if using character
                    // if (_hasAnimator)
                    // {
                    //     _animator.SetBool(_animIDFreeFall, true);
                    // }
                }

                // if we are not grounded, do not jump
                //_keyInput.Jump = false;
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
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
            
            _animator.SetBool("IsGrounded", Grounded);
        }
        
        private void _MovePlayer()
        {
            if (_keyInput.CameraLocked)
            {
            }
            else
            {
                float targetSpeed = _keyInput.Sprint ? SprintSpeed : MoveSpeed;
                
                if (_keyInput.Move == Vector2.zero) targetSpeed = 0.0f;

                float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;
                float speedOffset = 0.1f;
                float inputMagnitude = _keyInput.Move.magnitude;

                if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                    currentHorizontalSpeed > targetSpeed + speedOffset)
                {
                    _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                        Time.deltaTime * SpeedChangeRate);
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

                if (_keyInput.Move != Vector2.zero)
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

                // move the player
                _characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }
        }
    }
}