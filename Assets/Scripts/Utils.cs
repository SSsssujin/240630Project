using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INeverFall
{
    public static class Utils
    {
        public static void SetAnimatorTrigger(this Animator animator, AnimatorTrigger trigger)
        {
            //Debug.Log($"SetAnimatorTrigger: {trigger} - {( int )trigger}");
            
            animator.SetInteger(AnimationID.TriggerNumber, (int)trigger);
            animator.SetTrigger(AnimationID.Trigger);
        }
    }
}
