using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INeverFall.Player
{
    public static class AnimationID
    {
        public static readonly int TriggerNumber = Animator.StringToHash("TriggerNumber");
        public static readonly int Trigger = Animator.StringToHash("Trigger");
        
        public static readonly int Jumping = Animator.StringToHash("Jumping");
        public static readonly int VelocityX = Animator.StringToHash("Velocity X");
        public static readonly int VelocityZ = Animator.StringToHash("Velocity Z");
        public static readonly int Action = Animator.StringToHash("Action");
        public static readonly int Moving = Animator.StringToHash("Moving");
        public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        public static readonly int AirborneSpeed = Animator.StringToHash("AirborneSpeed");
    }

    public enum AnimatorTrigger
    {
        NoTrigger = 0,
        AttackTrigger = 4,
        AttackDualTrigger = 6,
        JumpTrigger = 18,
    }
}

namespace INeverFall.Monster
{
    public static class AnimationID
    {
        public static readonly int IsMoving = Animator.StringToHash("IsMoving");
        public static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        public static readonly int DamageTrigger = Animator.StringToHash("DamageTrigger");
        public static readonly int DeadTrigger = Animator.StringToHash("DeadTrigger");
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int Damage = Animator.StringToHash("Damage");
    }

    // BossAnimation enum을 파라미터로 각 애니메이션의 이름을 가져올 수 있다. - Utils class 참고
    // int 값이 명시되어있는 항목들은 Animation 트랜지션 조건이니까 잘 관리해 줘야 된다.  
    public enum BossAnimation
    {
        None = -1,
        
        // Melee
        GroundAttack = 0,
        ArmSwingAttack = 1,
        
        // Long range
        Dash = 5,
        ThrowStone = 6,
        
        HitLeftShoulder,
        HitRightShoulder,
    }
}