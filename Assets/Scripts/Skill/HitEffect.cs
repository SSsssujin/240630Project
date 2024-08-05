using System.Collections;
using System.Collections.Generic;
using INeverFall.Manager;
using UnityEngine;

namespace INeverFall
{
    public class HitEffect : MonoBehaviour
    {
        public void Activate(Vector3 position)
        {
            SoundManager.Instance.PlayAudio("BossHit");
            transform.position = position;
            StartCoroutine(nameof(_cStartDestroy));
        }

        private IEnumerator _cStartDestroy()
        {
            yield return new WaitForSeconds(1);
            ResourceManager.Instance.Destroy(gameObject);
        }
    }
}