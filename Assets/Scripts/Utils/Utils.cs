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
                _ => throw new ArgumentOutOfRangeException(nameof(animation), animation, null)
            };
            return name;
        }
        
        public static AnimationClip GetAnimationClipByName(Animator animator, string clipName)
        {
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;

            foreach (var clip in ac.animationClips)
            {
                Debug.Log(clip.name);
                
                if (String.Equals(clip.name, clipName))
                {
                    return clip;
                }
            }
            return null;
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
    
    public static class Layer
    {
        public static int None = 0;
        
        public static int Player = 3;

        public static int Enemy = 6;
        public static int Rock = 7;
        public static int TempCast = 8;
        public static int Walkable = 9;
    }

}
