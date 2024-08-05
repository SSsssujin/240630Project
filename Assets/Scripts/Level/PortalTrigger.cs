using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

namespace INeverFall
{
    public class PortalTrigger : MonoBehaviour
    {
        private bool _hasActivated;
        private Portal _portal;

        void Start()
        {
            _portal = transform.parent.GetComponentInChildren<Portal>(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasActivated) return;
            
            if (other.CompareTag("Player"))
            {
                _portal.Activate();
                _hasActivated = true;
            }
        }
    }
}