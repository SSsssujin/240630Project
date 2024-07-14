using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace INeverFall
{
    public class CameraSettings : MonoBehaviour
    {
        enum CameraType
        {
            FPS,
            TPS
        }
        
        [SerializeField] 
        private Transform _target;

        [SerializeField] 
        private CameraType _type;

        private CinemachineCamera _firstCamera;
        private CinemachineCamera _thirdCamera;

        private void OnValidate()
        {
            _Cache();
            _firstCamera.Target.TrackingTarget = _target;
            _thirdCamera.Target.TrackingTarget = _target;
        }

        private void Start()
        {
            _Cache();
        }

        private void _Cache()
        {
            _firstCamera ??= transform.Find("FPS").GetComponentInChildren<CinemachineCamera>();
            _thirdCamera ??= transform.Find("TPS").GetComponentInChildren<CinemachineCamera>();
        }

        public Transform CurrentCamera => transform.GetChild((int)_type);
    }
}