using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5;
    public float Gravity = 10;
    public float JumpSpeed = 10;

    private Vector2 _moveDirection;
    private Vector3 _newPosition;
    
    private bool _isGrounded;
    private bool _isJumpInput;
    private bool _isJumpable;
    
    private float _forwardSpeed;
    private float _verticalSpeed;
    private float _desiredForwardSpeed;
    
    private CharacterController _characterController;
    
    private const float _groundedRayDistance = 0.75f;
    private const float _stickingGravityProportion = 0.3f;
    private const float _jumpAbortSpeed = 10f;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

    private void FixedUpdate()
    {
        _CalculateMovement();
        //_SetPlayerRotation();
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
        Debug.Log(moveInput);

        //_desiredForwardSpeed = moveInput.magnitude * MaxForwardSpeed;
        //float acceleration = IsMoveInput ? 20 : 25;
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
    
    private void _MoveCharacter()
    {
        Vector3 position = transform.position;
        
        _newPosition.Set(_moveDirection.x, 0, _moveDirection.y);
        _newPosition *= MoveSpeed;
        
        if (_isGrounded)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position + Vector3.up * _groundedRayDistance, -Vector3.up);
            if (Physics.Raycast(ray, out hit, _groundedRayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
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
}
