using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

namespace INeverFall.Player
{
    public class FPSPlayerController : MonoBehaviour
    {
        [Header("Player movement")]
        public float MoveSpeed = 4.0f;
        public float SprintSpeed = 6.0f;
        public float RotationSpeed = 1.0f;
        public float SpeedChangeRate = 10.0f;
        
        [Space(10)]
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;

        [Space(10)]
        public float JumpTimeout = 0.1f;
        public float FallTimeout = 0.15f;

        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.5f;
        public LayerMask GroundLayers;
        
        [Header("Camera rotation")]
        public GameObject CinemachineCameraTarget;
        public float TopClamp = 90.0f;
        public float BottomClamp = -90.0f;
        
        private const float _threshold = 0.01f;
        
        // player
        private float _speed;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private float _cinemachineTargetPitch;
        
        private PlayerInputs _input;
        private CharacterController _controller;
        private GameObject _mainCamera;
        
        private void Start()
        {
            _input = GetComponent<PlayerInputs>();
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            _JumpAndGravity();
            _GroundedCheck();
            _Move();
        }

        private void LateUpdate()
        {
            //_RotateCamera();
        }

        private void _JumpAndGravity()
        {
            
        }

        private void _GroundedCheck()
        {
            
        }

        private void _Move()
        {
            float targetSpeed = _input.Sprint ? SprintSpeed : MoveSpeed;
            
            if (_input.Move == Vector2.zero) targetSpeed = 0.0f;
            
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
           
            float speedOffset = 0.1f;
            float inputMagnitude = _input.Move.magnitude;
            
            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.Move.x, 0.0f, _input.Move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.Move != Vector2.zero)
            {
                // move
                inputDirection = transform.right * _input.Move.x + transform.forward * _input.Move.y;
            }

            // move the player
            _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        private void _RotateCamera()
        {
            if (_input.Look.sqrMagnitude >= _threshold)
            {
                float deltaTimeMultiplier = 1.0f;
				
                _cinemachineTargetPitch += _input.Look.y * RotationSpeed * deltaTimeMultiplier;
                _rotationVelocity = _input.Look.x * RotationSpeed * deltaTimeMultiplier;

                // clamp pitch rotation
                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                // Update Cinemachine camera target pitch
                CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

                // rotate the player left and right
                transform.Rotate(Vector3.up * _rotationVelocity);
            }
        }
        
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}
