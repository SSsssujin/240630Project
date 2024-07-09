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
            _controller.NavMeshAgent.SetDestination(_controller.Target.transform.position);
        }

        protected override void _Exit()
        {
            
        }

        protected override float CooldownTime { get; set; } = 10;

        public override bool IsReady => _isCoolTimeEnded && !_controller.IsTargetWithinDistance(_controller.AttackDistance);
    }
}