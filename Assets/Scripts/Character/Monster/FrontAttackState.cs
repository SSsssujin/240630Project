using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INeverFall.Monster
{
    public class FrontAttackState : AttackState
    {
        public FrontAttackState(BossMonster controller) : base(controller)
        {
        }
        
        protected override void _Enter()
        {
            _animation = _GetRandomFrontAttackAnimation();
            //_SetGroundPosition();
        }

        protected override void _Update()
        {
            
        }

        protected override void _Exit()
        {
            
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

        private void _SetGroundPosition()
        {
            var refPosition = _controller.transform.position + Vector3.forward * 5;
            if (Physics.Raycast( refPosition, Vector3.down, out var hit, Mathf.Infinity))
            {
                _controller.GroundAttackHandPosition = hit.point;
            }
        }

        protected override float CooldownTime { get; set; } = 0f;
        
        public override bool IsReady => _isCoolTimeEnded && _controller.IsTargetWithinAttackRange;
    }
}