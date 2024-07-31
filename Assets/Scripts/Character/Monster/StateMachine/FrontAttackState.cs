using System.Collections;
using System.Collections.Generic;
using INeverFall.Manager;
using UnityEngine;

namespace INeverFall.Monster
{
    public class FrontAttackState : AttackState
    {
        private const int _attackPowerMultiplier = 0;
        
        private Skill _skill;
        
        public FrontAttackState(BossMonster controller) : base(controller)
        {
        }
        
        protected override void _Enter()
        {
            _animation = _GetRandomFrontAttackAnimation();

            string skillId = _animation.ToString();
            if (ResourceManager.Instance.Instantiate(skillId).TryGetComponent(out _skill))
            {
                _skill.Initialize(_controller, _controller.AttackPower + _attackPowerMultiplier);
                _controller.ActivateEffect(_animation);
            }
        }

        protected override void _Update()
        {
            
        }

        protected override void _Exit()
        {
            ResourceManager.Instance.Destroy(_skill.gameObject);
            _controller.DeactivateEffect(_animation);
        }

        // FrontAttack에 해당하는 애니메이션 중에서 랜덤으로 하나만 뽑기
        private BossAnimation _GetRandomFrontAttackAnimation()
        {
            BossAnimation[] frontAttackAnimations = 
            {
                BossAnimation.GroundAttack,
                BossAnimation.ArmSwingAttack
            };
            return frontAttackAnimations[Random.Range(0, frontAttackAnimations.Length)];
        }

        protected override float CooldownTime { get; set; } = 0f;
        
        public override bool IsReady => _isCoolTimeEnded && _controller.IsTargetWithinAttackRange;
    }
}