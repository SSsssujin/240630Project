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
    }
}