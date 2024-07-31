using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INeverFall.Monster
{
    public class BossBodyEffect : MonoBehaviour
    {
        [SerializeField] private BossAnimation _effect;

        private ParticleSystem _fx;

        private void OnValidate()
        {
            _fx = GetComponent<ParticleSystem>();
        }

        public void ActivateEffect()
        {
            _fx.gameObject.SetActive(true);
            _fx.Play();
        }
        
        public void DeactivateEffect()
        {
            _fx.Stop();
        }

        public BossAnimation EffectType => _effect;
    }
}