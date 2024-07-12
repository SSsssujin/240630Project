using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace INeverFall
{
    public class PlayerInputs : MonoBehaviour
    {
        [SerializeField] private Vector2 _move;
        [SerializeField] private Vector2 _look;
        [SerializeField] private Vector2 _zoom;
        
        [SerializeField] private bool _jump;
        [SerializeField] private bool _sprint;
        [SerializeField] private bool _cameraLocked;
        
        public void OnMove(InputValue value)
        {
            _move = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            _look = value.Get<Vector2>();
        }

        public void OnZoom(InputValue value)
        {
            _zoom = value.Get<Vector2>();
        }

        public void OnJump(InputValue value)
        {
            _jump = value.isPressed;
        }

        public void OnSprint(InputValue value)
        {
            _sprint = value.isPressed;
        }

        public void OnCameraLock(InputValue value)
        {
            _cameraLocked = value.isPressed;
        }
        
        public Vector2 Move => _move;
        public Vector2 Look => _look;
        public Vector2 Zoom => _zoom;
        
        public bool Jump => _jump;
        public bool Sprint => _sprint;
        public bool CameraLocked => _cameraLocked;
    }
}