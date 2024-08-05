using System;
using INeverFall;
using UnityEngine;

namespace INeverFall
{
    public class Portal : MonoBehaviour, IDamageable
    {
        private const int _maxHp = 800;
        
        private int _hp;
        private bool _isActivated;

        private ParticleSystem _fx;

        public void Activate()
        {
            // Caching
            _hp = _maxHp;
            _fx = GetComponent<ParticleSystem>();
            
            // Activate
            gameObject.SetActive(true);
            _isActivated = true;
        }

        public void OnDamage(CharacterBase owner, int damage)
        {
            if (!_isActivated) return;
            
            _hp -= (damage / 2);

            transform.localScale = Vector3.one * ((float)_hp / _maxHp);

            if (_hp <= 400)
            {
                _hp = 0;
                _Dead();
            }
        }

        private void _Dead()
        {
            _fx.Stop();
            gameObject.SetActive(false);
            GameManager.Instance.IsBossRoomOpened = true;

            var portalClearText = GameManager.Instance.PortalTextController;
            var bossRoomDoor = GameManager.Instance.BossRoomDoor;
            portalClearText.Execute();
            bossRoomDoor.Open();
        }
    }
}