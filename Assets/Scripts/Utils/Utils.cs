using System.Collections;
using System.Collections.Generic;
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
                    attackTime = 0.5f;
                    break;
                case WeaponType.TwoHandSword:
                    attackTime = 0.75f;
                    break;
            }

            return attackTime;
        }
    }
}
