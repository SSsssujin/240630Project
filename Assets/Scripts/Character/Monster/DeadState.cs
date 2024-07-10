using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INeverFall.Monster
{
    public class DeadState : IState
    {
        private readonly BossMonster _controller;
        
        public DeadState(BossMonster controller)
        {
            _controller = controller;
        }
        
        public void Enter()
        {
            _controller.Animator.SetTrigger(AnimationID.DeadTrigger);
        }

        public void Update()
        {
        }

        public void Exit()
        {
        }
    }
}