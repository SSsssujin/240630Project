using UnityEngine;

namespace INeverFall
{

    public static class Extensions
    {
        public static void SetAnimatorTrigger(this Animator animator, AnimatorTrigger trigger)
        {
            //Debug.Log($"SetAnimatorTrigger: {trigger} - {( int )trigger}");
            
            animator.SetInteger(AnimationID.TriggerNumber, (int)trigger);
            animator.SetTrigger(AnimationID.Trigger);
        }

        public static void ResetLocal(this Transform origin)
        {
            origin.localPosition = Vector3.zero; 
            origin.localRotation = Quaternion.identity; 
            origin.localScale = Vector3.one;
        }

    }
}