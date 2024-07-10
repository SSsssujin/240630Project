using UnityEngine;
using UnityEngine.InputSystem;

namespace INeverFall.Player
{
    [RequireComponent(typeof(Animator), typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputs), typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputs _input;

        private FirstPersonMovement _fpsMovement;
        private ThirdPersonMovement _tpsMovement;
        
        private void Start()
        {
            _input = GetComponent<PlayerInputs>();
            
            _fpsMovement = new();
            _tpsMovement = new();
        }

        private void Update()
        {
            _CalculateMovement();
            _SetPlayerRotation();
            _MovePlayer();
        }

        private void _SetPlayerRotation()
        {
            
        }

        private void _CalculateMovement()
        {
            
        }

        private void _MovePlayer()
        {
            if (_input.CameraLocked)
            {
                _fpsMovement.Move();
            }
            else
            {
                _tpsMovement.Move();
            }
        }
    }
}