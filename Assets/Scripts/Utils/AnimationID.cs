using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INeverFall.Player
{
    public static class AnimationID
    {
        public static readonly int Jump = Animator.StringToHash("Jump");
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int Damage = Animator.StringToHash("Damage");
        public static readonly int Death = Animator.StringToHash("Death");
        public static readonly int VelocityX = Animator.StringToHash("Velocity X");
        public static readonly int VelocityZ = Animator.StringToHash("Velocity Z");
        public static readonly int AttackID = Animator.StringToHash("AttackID");
        public static readonly int IsMoving = Animator.StringToHash("IsMoving");
        public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        public static readonly int IsCritical = Animator.StringToHash("IsCritical");
    }
}

namespace INeverFall.Monster
{
    public static class AnimationID
    {
        public static readonly int IsMoving = Animator.StringToHash("IsMoving");
        public static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        public static readonly int DamageTrigger = Animator.StringToHash("DamageTrigger");
        public static readonly int RiseTrigger = Animator.StringToHash("RiseTrigger");
        public static readonly int DeadTrigger = Animator.StringToHash("DeadTrigger");
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int Damage = Animator.StringToHash("Damage");
    }
}