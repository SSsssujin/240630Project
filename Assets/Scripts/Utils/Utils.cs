using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall.Monster;
using UnityEngine;

namespace INeverFall
{
    public static class Utils
    {

        public static float AttackDuration(WeaponType weapon)
		{
			float duration = 1f;

            switch (weapon)
            {
                case WeaponType.Unarmed:
                    duration = 0.75f;
                    break;
                case WeaponType.TwoHandSword:
                    duration = 1.1f;
                    break;
            }
            
			return duration;
		}

        public static float GetPreciseAttackTime(WeaponType weapon)
        {
            float attackTime = 0.5f;
            
            switch (weapon)
            {
                case WeaponType.Unarmed:
                    attackTime = 0.65f;
                    break;
                case WeaponType.TwoHandSword:
                    attackTime = 0.5f;
                    break;
            }

            return attackTime;
        }

        public static string GetBossAttackAnimationName(Monster.BossAnimation animation)
        {
            string name = animation switch
            {
                BossAnimation.GroundAttack => "atk_ground02",
                BossAnimation.ArmSwingAttack => "atk_armSwing",
                BossAnimation.Dash => "atk_dash",
                BossAnimation.ThrowStone => "atk_throwStone",
                BossAnimation.HitLeftShoulder => "hit_left_shoulder",
                BossAnimation.HitRightShoulder => "hit_right_shoulder",
                _ => throw new ArgumentOutOfRangeException(nameof(animation), animation, null)
            };
            return name;
        }

        
        public static AnimationClip GetAnimationClipByType(Animator animator, BossAnimation animation)
        {
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            string clipName = GetBossAttackAnimationName(animation);
            foreach (var clip in ac.animationClips)
            {
                if (String.Equals(clip.name, clipName))
                {
                    return clip;
                }
            }
            return null;
        }
    }
    
    public struct Layer
    {
        public const int None = 0;
        public const int Player = 3;
        public const int Enemy = 6;
        public const int Rock = 7;
        public const int TempCast = 8;
        public const int Walkable = 9;
        public const int Environment = 10;
    }

}
