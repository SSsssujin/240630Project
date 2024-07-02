using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INeverFall.Monster
{
    public class AttackState : State
    {
        private readonly BossMonster _controller;

        public AttackState(BossMonster controller)
        {
            _controller = controller;
        }
        
        public override void Enter()
        {
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
        }
    }
}