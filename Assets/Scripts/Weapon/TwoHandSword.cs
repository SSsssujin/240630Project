using System.Collections;
using INeverFall.Manager;
using UnityEngine;

namespace INeverFall
{
    public class TwoHandSword : Weapon
    {
        protected override void _SetWeaponType() => _weaponType = WeaponType.TwoHandSword;
        

        public override void DoAttack()
        {
            StartCoroutine(nameof(_cStartAttack));
        }

        private IEnumerator _cStartAttack()
        {
            yield return new WaitForSeconds(_attackTiming);
            
            Debug.Log("Now");
            
            var skill = ResourceManager.Instance.Instantiate("Skill");
            skill.transform.SetParent(_slashRoot);
            skill.transform.ResetLocal();
        }
    }
}

