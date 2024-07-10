using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace INeverFall.Player
{
    public class TPSPlayerController : PlayerController
    {
        public float MoveSpeed = 5;
        public float MaxForwardSpeed = 5f;
        public float Gravity = 10;
        public float JumpSpeed = 10;
        public float RotationSmoothTime = 0.12f;

        private Vector2 _moveDirection;
        private Vector3 _newPosition;
        private Quaternion _targetRotation;

        private bool _isGrounded;
        private bool _isJumpInput;
        private bool _isReadyToJump;
        
        private bool _IsJumpable => _isJumpInput && _isGrounded;

        private float _forwardSpeed;
        private float _verticalSpeed;
        private float _desiredForwardSpeed;
        private float _angleDiff;
        private float _targetRotationY;
        private float _rotationVelocity;

        private CharacterController _characterController;
        private Animator _animator;
        
        // Camera
        private GameObject _mainCamera;

        private float _attackTimeDelay;
        private float _lastAttackTime;

        private Weapon _weapon;
        private PlayerCharacter _playerCharacter;
        
        private const float _groundedRayDistance = 1.5f;
        private const float _stickingGravityProportion = 0.3f;
        private const float _jumpAbortSpeed = 10f;
        private const float _airborneTurnSpeedProportion = 5.4f;
        private const float _inverseOneEighty = 1f / 180f;

        private bool _IsMoveInput => !Mathf.Approximately(_moveDirection.sqrMagnitude, 0f);
        private bool _CanAttack => Time.time > _lastAttackTime + _attackTimeDelay && _isGrounded;

        public Vector3 PlayerForward => Quaternion.Euler(0.0f, _targetRotationY, 0.0f) * Vector3.forward;

        private void Start()
        {
            _playerCharacter = GetComponent<PlayerCharacter>();
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _weapon = GetComponentInChildren<Weapon>();
            _mainCamera ??= Camera.main?.gameObject;
            _attackTimeDelay = Utils.AttackDuration(_weapon.WeaponType);
        }

        private void FixedUpdate()
        {
            _CalculateMovement();
            _SetPlayerRotation();
            _MoveCharacter();
        }

        private void Update()
        {
            if (!_isAttackable && _CanAttack)
            {
                _isAttackable = true;
            }
        }

        #region  [ Input Method ]

        private void OnMove(InputValue value)
        {
            _moveDirection = value.Get<Vector2>();
        }

        private void OnJump()
        {
            //_isJumpInput = true;

            if (_isGrounded)
            {
                _verticalSpeed = JumpSpeed;
                _isGrounded = false;
                _isReadyToJump = false;
                
                _animator.SetInteger(AnimationID.Jumping, 1);
                _animator.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
            }
        }

        private bool _isAttackable;

        private void OnAttack()
        {
            if (_isAttackable)
            {
                _isAttackable = false;
                _lastAttackTime = Time.time;

                if (_IsMoveInput)
                {
                    // ?
                    //_animator.SetAnimatorTrigger(AnimatorTrigger.AttackTrigger);
                    //_animator.SetInteger(AnimationID.Action,1);
                }
                else
                {
                    _animator.SetAnimatorTrigger(AnimatorTrigger.AttackDualTrigger);
                    _animator.SetInteger(AnimationID.Action, UnityEngine.Random.Range(1, 12));
                }
                
                _weapon.DoAttack(_playerCharacter, PlayerForward);
            }
        }

        #endregion

        private void _CalculateMovement()
        {
            // Forward movement
            Vector2 moveInput = _moveDirection;
            if (moveInput.sqrMagnitude > 1f)
            {
                moveInput.Normalize();
            }

            _desiredForwardSpeed = moveInput.magnitude * MaxForwardSpeed;
            float acceleration = _IsMoveInput ? 20 : 25;
            _forwardSpeed = Mathf.MoveTowards(_forwardSpeed, _desiredForwardSpeed, acceleration * Time.deltaTime);

            // Vertical movement
            if (_isGrounded) // Ground
            {
                _verticalSpeed = -Gravity * _stickingGravityProportion;

                if (_IsJumpable)
                {
                    _verticalSpeed = JumpSpeed;
                    _isGrounded = false;

                    _animator.SetInteger(AnimationID.Jumping, 1);
                    _animator.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
                }
                else
                {
                    _animator.SetInteger(AnimationID.Jumping, 0);
                    _animator.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
                }
            }
            else // Airborne
            {
                if (_verticalSpeed > 0.0f)
                {
                    _verticalSpeed -= _jumpAbortSpeed * Time.deltaTime;
                }

                if (Mathf.Approximately(_verticalSpeed, 0f))
                {
                    _verticalSpeed = 0f;
                }

                // Apply gravity
                _verticalSpeed -= Gravity * Time.deltaTime;
            }
        }

        private void _SetPlayerRotation()
        {
            // Normalize input direction
            Vector2 moveInput = _moveDirection;
            Vector3 localMovementDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

            if (_IsMoveInput)
            {
                _targetRotationY = Mathf.Atan2(localMovementDirection.x, localMovementDirection.z) * Mathf.Rad2Deg +
                                   _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotationY, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }

        private void _MoveCharacter()
        {
            Vector3 position;
            // Vector3 position = transform.position;
            //
            // position.Set(_moveDirection.x, 0, _moveDirection.y);
            // position *= MoveSpeed;

            if (_isGrounded)
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position + Vector3.up * _groundedRayDistance, -Vector3.up);
                if (Physics.Raycast(ray, out hit, _groundedRayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    position = Vector3.ProjectOnPlane(_animator.deltaPosition * (_forwardSpeed * Time.deltaTime), hit.normal);
                }
                else
                {
                    position = _animator.deltaPosition; //_newPosition * (_forwardSpeed * Time.deltaTime);
                }
                
            }
            else
            {
                position = _forwardSpeed * transform.forward * Time.deltaTime;
            }

            position += _verticalSpeed * Vector3.up * Time.deltaTime;
            //_characterController.Move(position);


             Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotationY, 0.0f) * Vector3.forward;
             _characterController.Move(targetDirection.normalized * (_desiredForwardSpeed * Time.deltaTime) +
                                       new Vector3(0.0f, _verticalSpeed, 0.0f) * Time.deltaTime);
            
            _isGrounded = _characterController.isGrounded;
            _isJumpInput = false;
        }

        private void OnAnimatorMove()
        {
            _animator.SetBool(AnimationID.IsGrounded, _isGrounded);

            if (!_isGrounded)
            {
                _animator.SetFloat(AnimationID.AirborneSpeed, _verticalSpeed);
            }
            
            if (_moveDirection.magnitude > 0) 
            {
                _animator.SetFloat(AnimationID.VelocityX, _moveDirection.x);
                _animator.SetFloat(AnimationID.VelocityZ, _moveDirection.y);
                _animator.SetBool(AnimationID.Moving, true);
            }
            else 
            {
                _animator.SetFloat(AnimationID.VelocityX, 0f);
                _animator.SetFloat(AnimationID.VelocityZ, 0f);
                _animator.SetBool(AnimationID.Moving, false);
            }
        }
    }
}