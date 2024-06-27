using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INeverFall
{
    public class Skill : MonoBehaviour
    {
        [SerializeField] private float _speed = 30;
        [SerializeField] private float _attackPower = 50;
        
        private bool _isActivate;
        private Vector3 _attackDirection;
        
        public void Initialize(Transform parent, Vector3 direction)
        {
            //transform.SetParent(parent);
            //transform.ResetLocal();

            transform.position = parent.position + new Vector3(0, -1f, 0);
            _attackDirection = direction;
            _isActivate = true;
        }

        private void Update()
        {
            if (_isActivate)
            {
                transform.position += _attackDirection * _speed * Time.deltaTime;
            }
        }
    }
}
