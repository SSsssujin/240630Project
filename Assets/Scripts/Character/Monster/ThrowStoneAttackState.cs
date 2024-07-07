using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall.Monster;
using UnityEngine;

namespace INeverFall.Monster
{
    public class ThrowStoneAttackState : AttackState
    {
        public ThrowStoneAttackState(BossMonster controller) : base(controller)
        {
            _animation = BossAnimation.ThrowStone;
        }

        protected override void _Enter()
        {
        }

        protected override void _Update()
        {
            // // Define a small epsilon value to create a time range for comparison
            // float epsilon = 0.005f;
            //
            // if (Mathf.Abs(_entranceTimer - 1.0f) < epsilon)
            // {
            //     ActionStarted?.Invoke();
            // }
            // if (Mathf.Abs(_entranceTimer - 4.75f) < epsilon)
            // {
            //     StoneThrown?.Invoke();
            // }
        }

        protected override void _Exit()
        {
            
        }
        
        //private void 

        public event Action ActionStarted;
        public event Action StoneThrown;

        protected override float CooldownTime { get; set; } = 30f;
    }
}