using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace INeverFall.Player
{
    public class PlayerController : MonoBehaviour
    {
        public float MoveSpeed = 5;
        public float MaxForwardSpeed = 5f;
        public float Gravity = 10;
        public float JumpSpeed = 10;
        public float MinTurnSpeed = 400f;      
        public float MaxTurnSpeed = 1200f;  

        private Vector2 _moveDirection;
        private Vector3 _newPosition;
        private Quaternion _targetRotation;

        private bool _isGrounded;
        private bool _isJumpInput;
        private bool _isJumpable;

        private float _forwardSpeed;
        private float _verticalSpeed;
        private float _desiredForwardSpeed;
        private float _angleDiff;

        private CharacterController _characterController;
        private Animator _animator;
        
        // Camera
        private CinemachineCamera _camera;
        private CinemachineOrbitalFollow _cinemachineOrbitalFollow;

        private const float _groundedRayDistance = 0.75f;
        private const float _stickingGravityProportion = 0.3f;
        private const float _jumpAbortSpeed = 10f;
        private const float _airborneTurnSpeedProportion = 5.4f;
        private const float _inverseOneEighty = 1f / 180f;

        private bool _IsMoveInput => !Mathf.Approximately(_moveDirection.sqrMagnitude, 0f);

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            
            // temp
            var camTr = GameObject.Find("FreeLookCamera");
            _camera = camTr.GetComponent<CinemachineCamera>();
            _cinemachineOrbitalFollow = camTr.GetComponent<CinemachineOrbitalFollow>();
        }

        private void FixedUpdate()
        {
            _CalculateMovement();
            _SetPlayerRotation();
            _MoveCharacter();
        }

        private void OnMove(InputValue value) // Vector2
        {
            _moveDirection = value.Get<Vector2>();
        }

        private void OnJump()
        {
            _isJumpInput = true;
        }

        private void _CalculateMovement()
        {
            // Forward movement
            Vector2 moveInput = _moveDirection;
            if (moveInput.sqrMagnitude > 1f)
            {
                moveInput.Normalize();
            }

            //_desiredForwardSpeed = moveInput.magnitude * MaxForwardSpeed;
            //float acceleration = _IsMoveInput ? 20 : 25;
            //_forwardSpeed = Mathf.MoveTowards(_forwardSpeed, _desiredForwardSpeed, acceleration * Time.deltaTime);

            // Vertical movement
            if (!_isJumpInput && _isGrounded)
                _isJumpable = true;

            if (_isGrounded) // Ground
            {
                _verticalSpeed = -Gravity * _stickingGravityProportion;

                if (_isJumpInput && _isGrounded)
                {
                    _verticalSpeed = JumpSpeed;
                    _isGrounded = false;
                    _isJumpable = false;
                }
            }
            else // Airborne
            {
                if (!_isJumpInput && _verticalSpeed > 0.0f)
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
            Vector2 moveInput = _moveDirection;
            Vector3 localMovementDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            
            Vector3 forward = Quaternion.Euler(0f, _cinemachineOrbitalFollow.HorizontalAxis.Value, 0f) * Vector3.forward;
            forward.y = 0f;
            forward.Normalize();

            Quaternion targetRotation;
            
            // If the local movement direction is the opposite of forward then the target rotation should be towards the camera.
            if (Mathf.Approximately(Vector3.Dot(localMovementDirection, Vector3.forward), -1.0f))
            {
                targetRotation = Quaternion.LookRotation(-forward);
            }
            else
            {
                // Otherwise the rotation should be the offset of the input from the camera's forward.
                Quaternion cameraToInputOffset = Quaternion.FromToRotation(Vector3.forward, localMovementDirection);
                targetRotation = Quaternion.LookRotation(cameraToInputOffset * forward);
            }

            // The desired forward direction of Ellen.
            Vector3 resultingForward = targetRotation * Vector3.forward;
            
            float angleCurrent = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
            float targetAngle = Mathf.Atan2(resultingForward.x, resultingForward.z) * Mathf.Rad2Deg;

            _angleDiff = Mathf.DeltaAngle(angleCurrent, targetAngle); 
            _targetRotation = targetRotation;
            
            if (_IsMoveInput)
                _UpdateOrientation();
        }

        private void _UpdateOrientation()
        {
            Vector3 localInput = new Vector3(_moveDirection.x, 0f, _moveDirection.y);
            float groundedTurnSpeed = Mathf.Lerp(MaxTurnSpeed, MinTurnSpeed, _forwardSpeed /_desiredForwardSpeed);
            float actualTurnSpeed = _isGrounded ? groundedTurnSpeed : Vector3.Angle(transform.forward, localInput) * _inverseOneEighty * _airborneTurnSpeedProportion * groundedTurnSpeed;
            _targetRotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, actualTurnSpeed * Time.deltaTime);

            transform.rotation = _targetRotation;
        }

        private void _MoveCharacter()
        {
            Vector3 position = transform.position;

            _newPosition.Set(_moveDirection.x, 0, _moveDirection.y);
            _newPosition *= MoveSpeed;

            if (_isGrounded)
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position + Vector3.up * _groundedRayDistance, -Vector3.up);
                if (Physics.Raycast(ray, out hit, _groundedRayDistance, Physics.AllLayers,
                        QueryTriggerInteraction.Ignore))
                {
                    position = Vector3.ProjectOnPlane(_newPosition * (_forwardSpeed * Time.deltaTime), hit.normal);
                }
                else
                {
                    position = _newPosition * (_forwardSpeed * Time.deltaTime);
                }
                
            }
            else
            {
                position = _newPosition * (_forwardSpeed * Time.deltaTime);
            }

            position += Vector3.up * (_verticalSpeed * Time.deltaTime);

            _characterController.Move(position);

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
        }
    }
}