using System.Collections;
using System.Collections.Generic;
using INeverFall;
using INeverFall.Manager;
using RPGCharacterAnims.Actions;
using UnityEngine;

namespace INeverFall.Monster
{
    public class DashAttackState : AttackState
    {
        private const int _attackPowerMultiplier = 5;
        private Skill _skill;

        public DashAttackState(BossMonster controller) : base(controller)
        {
            _animation = BossAnimation.Dash;
        }

        protected override void _Enter()
        {
            if (ResourceManager.Instance.Instantiate("Dash", _controller.transform).TryGetComponent(out _skill))
            {
                _skill.Initialize(_controller, _controller.AttackPower + _attackPowerMultiplier);
                _controller.ActivateEffect(_animation);
            }
            _controller.ActivateEffect(_animation);
        }

        protected override void _Update()
        {
            _controller.NavMeshAgent.SetDestination(_controller.Target.transform.position);
        }

        protected override void _Exit()
        {
            ResourceManager.Instance.Destroy(_skill.gameObject);
            _controller.DeactivateEffect(_animation);
        }

        protected override float CooldownTime { get; set; } = 5;

        public override bool IsReady => _isCoolTimeEnded; // && !_controller.IsTargetWithinDistance(_controller.AttackDistance);
    }
}