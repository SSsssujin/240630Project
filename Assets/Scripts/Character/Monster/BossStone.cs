using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStone : MonoBehaviour
{
    private const float _speed = 50f;
    
    private bool _isFlying;
    private Vector3 _direction;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        gameObject.SetActive(false);
    }

    public void Initialize(Vector3 throwDirection)
    {
        _rigidbody.isKinematic = true;
        _direction = throwDirection;
        gameObject.SetActive(true);
    }

    public void MoveInDirection()
    {
        _rigidbody.isKinematic = false;
        transform.SetParent(null);
        _rigidbody.velocity = _direction.normalized * _speed;
    }
}
