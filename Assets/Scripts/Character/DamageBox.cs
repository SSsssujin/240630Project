using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DamageBox : MonoBehaviour
{
    private Collider _collider;
    private CharacterBase _character;
    
    private void Start()
    {
        _collider = GetComponent<Collider>();
        _character = GetComponentInParent<CharacterBase>();
    }
}
