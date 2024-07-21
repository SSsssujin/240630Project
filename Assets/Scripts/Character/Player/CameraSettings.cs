using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace INeverFall
{
    public class CameraSettings : MonoBehaviour
    {
        [SerializeField] 
        private Transform _target;

        private CinemachineCamera _thirdCamera;

        private void OnValidate()
        {
            _Cache();
            _thirdCamera.Target.TrackingTarget = _target;
        }

        private void Start()
        {
            _Cache();
        }

        private void _Cache()
        {
            _thirdCamera ??= transform.GetComponentInChildren<CinemachineCamera>();
        }
    }
}