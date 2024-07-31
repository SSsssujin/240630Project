using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

namespace INeverFall.Monster
{
    public class GroggyState : IState
    {
        private readonly BossMonster _controller;
        private const float _duration = 5;
        
        private float _entranceTimer = 0;
        
        public GroggyState(BossMonster controller)
        {
            _controller = controller;
        }
        
        public void Enter()
        {
            _controller.Animator.SetTrigger(AnimationID.DamageTrigger);
            _controller.Animator.SetInteger(AnimationID.Damage, 0);
        }

        public void Update()
        {
            _entranceTimer += Time.deltaTime;

            if (_entranceTimer >= _duration)
            {
                _controller.StateMachine.TransitionTo(_controller.StateMachine.IdleState);
            }
        }

        public void Exit()
        {
            _entranceTimer = 0;
        }
    }
}