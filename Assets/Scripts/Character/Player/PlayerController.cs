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

        public float MoveSpeed;
        public float SprintSpeed;
        
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
        }

        private void Update()
        {
            _CalculateMovement();
            _SetPlayerRotation();
            _MovePlayer();
        }

        private void LateUpdate()
        {
            //_RotateCamera();
        }

        private static float _ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void _SetPlayerRotation()
        {
            
        }

        private void _CalculateMovement()
        {
            
        }

        private float _speed;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        
        public float SpeedChangeRate = 10.0f;
        public float RotationSmoothTime = 0.12f;

        private void _MovePlayer()
        {
            if (_keyInput.CameraLocked)
            {
            }
            else
            {
                float targetSpeed = _keyInput.Sprint ? SprintSpeed : MoveSpeed;
                
                if (_keyInput.Move == Vector2.zero) targetSpeed = 0.0f;

                //_animator.speed = _keyInput.Sprint ? 1.5f : 1;
                
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
                    
                    // float scaledVelocityX = direction.x * (targetSpeed / SprintSpeed);
                    // float scaledVelocityZ = direction.z * (targetSpeed / SprintSpeed);
                    //
                    // _animator.SetFloat(AnimationID.VelocityX, scaledVelocityX);
                    // _animator.SetFloat(AnimationID.VelocityZ, scaledVelocityZ);

                    _animator.SetFloat(AnimationID.VelocityX, direction.x * _speed);
                    _animator.SetFloat(AnimationID.VelocityZ, direction.z * _speed);
                }
                else
                {
                    _animator.SetFloat(AnimationID.VelocityX,0);
                    _animator.SetFloat(AnimationID.VelocityZ,0);
                }

                // move the player
                _characterController.Move(targetDirection * (_speed * Time.deltaTime));
            }
        }
    }
}