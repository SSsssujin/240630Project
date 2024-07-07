using System.Collections;
using System.Collections.Generic;
using INeverFall;
using RPGCharacterAnims.Actions;
using UnityEngine;

namespace INeverFall.Monster
{
    public class DashAttackState : AttackState
    {
        public DashAttackState(BossMonster controller) : base(controller)
        {
            _animation = BossAnimation.Dash;
        }
 
        protected override void _Enter()
        {
        }

        protected override void _Update()
        {

        }

        protected override void _Exit()
        {
            
        }

        protected override float CooldownTime { get; set; } = 25f;
    }
}